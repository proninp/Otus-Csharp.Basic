using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using File = System.IO.File;

namespace MTLServiceBot.Bot.Commands
{
    public partial class ServiceRequest
    {
        private static readonly Dictionary<CallbackCommand, string> _callBackCommands = new()
        {
            { CallbackCommand.ChangeStatus, TextConsts.CBCmdChangeStatus},
            { CallbackCommand.AddFile, TextConsts.CBCmdAddFile}
        };

        private InlineKeyboardMarkup? GetServiceTaskInlineButtons(ServiceTask? task)
        {
            if (task is null || task.TaskStatus is not (ServiceTaskStatus.New or ServiceTaskStatus.Execution))
                return null;

            var inlineKeyboard = new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(task.GetNextStatusStepDescription(),
                    $"{_callBackCommands[CallbackCommand.ChangeStatus]}{TextConsts.CBCmdDataSeparator}{task.Id}"),
                InlineKeyboardButton.WithCallbackData(TextConsts.CBCmdAddFileDescription,
                $"{_callBackCommands[CallbackCommand.AddFile]}{TextConsts.CBCmdDataSeparator}{task.Id}")
            };
            return new InlineKeyboardMarkup(inlineKeyboard);
        }

        private async Task HandleCallBackDataUpdateAsync(ITelegramBotClient botClient, TgUpdate update, Session session,
            List<ServiceTask> serviceTasksList, IReplyMarkup replyButtons)
        {
            (string cmdTaskId, CallbackCommand command) = ParseCallbackCommandData(update.CallbackQuery);
            if (string.IsNullOrEmpty(cmdTaskId) || command == CallbackCommand.None)
                return;

            var serviceTask = serviceTasksList.FirstOrDefault(st => st.Id == cmdTaskId);
            if (serviceTask is null)
            {
                _ = botClient.EditMessageReplyMarkupAsync(update.Chat.Id, update.Message.MessageId, replyMarkup: null);

                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdServiceTaskNotFound, cmdTaskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

            switch (command)
            {
                case CallbackCommand.ChangeStatus:
                    if (await ChangeTaskStatusApiRequestAsync(botClient, update, session, serviceTask))
                        _ = UpdateSingleTaskInfoAsync(botClient, update, session, serviceTask);
                    break;
                case CallbackCommand.AddFile:
                    AddNewFileCall(botClient, update, session, serviceTask.Id);
                    break;
            }
        }

        private (string, CallbackCommand) ParseCallbackCommandData(CallbackQuery? callback)
        {
            var command = CallbackCommand.None;
            if (callback is null || string.IsNullOrEmpty(callback.Data))
            {
                Program.ColoredPrint(TextConsts.CBCmdDataEmpty, ConsoleColor.Red); // TODO Logging
                return ("", command);
            }

            var data = callback.Data;
            var cbCmdPair = _callBackCommands.FirstOrDefault(cq => data.StartsWith(cq.Value));
            if (!data.Contains(TextConsts.CBCmdDataSeparator) || cbCmdPair.Equals(default(KeyValuePair<CallbackCommand, string>)))
            {
                Program.ColoredPrint(string.Format(TextConsts.CBCmdDataNotRecognized, data), ConsoleColor.Red); // TODO Logging
                return ("", command);
            }

            command = cbCmdPair.Key;
            var dataList = data.Split(TextConsts.CBCmdDataSeparator);
            if (dataList.Length != 2)
            {
                Program.ColoredPrint(string.Format(TextConsts.CBCmdDataUndefined, callback.Data), ConsoleColor.Red); // TODO Logging
                return ("", command);
            }
            return (dataList[1], command);
        }

        private async Task<bool> ChangeTaskStatusApiRequestAsync(ITelegramBotClient botClient, TgUpdate update,
            Session session, ServiceTask serviceTask)
        {
            var response = await _api.ChangeServiceTaskStatus(session, serviceTask);
            if (!response.IsSuccess)
            {
                if (response.Status == ApiResponseStatus.Unauthorized)
                    _ = botClient.EditMessageReplyMarkupAsync(update.Chat.Id, update.Message.MessageId, replyMarkup: null);
                _ = botClient.SendTextMessageAsync(update.Chat, response.Message, parseMode);
                return false;
            }
            return true;
        }

        private async Task<bool> UpdateSingleTaskInfoAsync(ITelegramBotClient botClient, TgUpdate update, Session session, ServiceTask task)
        {
            // Делаем запрос для получения обновленной информации по сервисной задаче
            var serviceTask = await UpdateSingleServiceTaskInfoAsync(session, task.Id);
            if (serviceTask is null)
            {
                _ = botClient.EditMessageReplyMarkupAsync(update.Chat.Id, update.Message.MessageId, replyMarkup: null);
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.SingleServiceRequestUpdateFailureMsg, task.RequestNo, task.TaskNo),
                    parseMode: ParseMode.Html);
                return false;
            }
            // Выводим обновленную информацию в то же сообщение, обновляем кнопки
            _ = botClient.EditMessageTextAsync(update.Chat.Id, update.Message.MessageId,
                serviceTask.ToMarkedDownString(),
                ParseMode.Html, replyMarkup: GetServiceTaskInlineButtons(serviceTask));
            return true;
        }

        private async Task<ServiceTask?> UpdateSingleServiceTaskInfoAsync(Session session, string taskId)
        {
            ServiceTask? serviceTask = null;
            if (string.IsNullOrEmpty(taskId) || !taskId.Contains(TextConsts.SingleTaskNumberFormatSeparator))
                return serviceTask;
            var taskIdParts = taskId.Split(TextConsts.SingleTaskNumberFormatSeparator);
            if (taskIdParts.Length != 2)
                return serviceTask;
            var response = await _api.GetServiceTask(session, taskIdParts[0], taskIdParts[1]);
            if (!response.IsSuccess)
                return serviceTask;

            try
            {
                serviceTask = JsonSerializer.Deserialize<List<ServiceTask>>(response.ResponseText)?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Program.ColoredPrint(ex.ToString(), ConsoleColor.Red); // TODO Logging
            }
            return serviceTask;
        }

        private void AddNewFileCall(ITelegramBotClient botClient, TgUpdate update, Session session, string taskId)
        {
            session.WorkFlowState = WorkFlow.ServiceRequestAddFile;
            session.WorkFlowTaskId = taskId;
            _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdAddFileCallMsgDescription, taskId),
                    parseMode: ParseMode.Html);
        }

        private async void HandleAddNewFileCallAsync(ITelegramBotClient botClient, TgUpdate update, Session session,
            List<ServiceTask> serviceTasksList, IReplyMarkup replyButtons)
        {
            if (!CheckAddNewFileCallParams(botClient, update, session, replyButtons, out string taskId))
                return;

            var serviceTask = serviceTasksList.FirstOrDefault(st => st.Id == taskId);
            if (serviceTask is null)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdServiceTaskNotFound, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

            string? fileId = GetAttachmentFileId(update);
            
            if (string.IsNullOrEmpty(fileId))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.AddFileHandleIdError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }
            var fileInfo = await botClient.GetFileAsync(fileId);
            if (fileInfo?.FilePath is null)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.AddFileHandleReceiveError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

            var filePath = fileInfo.FilePath;
            var filename = $"{serviceTask.RequestNo}_{serviceTask.TaskNo}_{Path.GetFileName(filePath)}";
            var destinationFilePath = Path.Combine(_downloadedFilesDirectory, update.From.Id.ToString(), filename);
            if (!await DownloadFileAsync(botClient, update, replyButtons, taskId, filePath, destinationFilePath))
                return;
            await AddNewTaskFileApiRequestAsync(botClient, update, session, serviceTask, filename, destinationFilePath);
        }

        private bool CheckAddNewFileCallParams(ITelegramBotClient botClient, TgUpdate update, Session session,
            IReplyMarkup replyButtons, out string taskId)
        {
            taskId = session.WorkFlowTaskId;
            if (session.WorkFlowState != WorkFlow.ServiceRequestAddFile || string.IsNullOrEmpty(taskId))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    TextConsts.CBCmdAddFileCallHandlerError,
                    parseMode: ParseMode.Html);
                return false;
            }

            if (!update.HasAttachment())
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdAddFileCallHandlerFileError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return false;
            }
            return true;
        }

        private string? GetAttachmentFileId(TgUpdate update)
        {
            string? fileId = null;
            switch (update.Message.Type)
            {
                case MessageType.Photo:
                    fileId = update.Message.Photo?.Last().FileId;
                    break;
                case MessageType.Video:
                    fileId = update.Message?.Video?.FileId;
                    break;
                case MessageType.Document:
                    fileId = update.Message?.Document?.FileId;
                    break;
            }
            return fileId;
        }

        private async Task<bool> DownloadFileAsync(ITelegramBotClient botClient, TgUpdate update, IReplyMarkup replyButtons,
            string taskId, string filePath, string destinationFilePath)
        {
            var dir = Path.GetDirectoryName(destinationFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using Stream fileStream = File.Create(destinationFilePath);
            await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
            if (!File.Exists(destinationFilePath))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.AddFileHandleDownloadError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return false;
            }
            return true;
        }

        private async Task AddNewTaskFileApiRequestAsync(ITelegramBotClient botClient, TgUpdate update, Session session,
            ServiceTask task, string filename, string destinationFilePath)
        {
            string base64Content = Convert.ToBase64String(File.ReadAllBytes(destinationFilePath));
            var response = await _api.AddNewFileToServiceTask(session, task, filename, base64Content);
            if (response.IsSuccess)
            {
                _ = botClient.SendTextMessageAsync(update.Chat, string.Format(TextConsts.AddFileHandleAddedMsg, task.Id, filename),
                    parseMode: ParseMode.Html);
                return;
            }
            _ = botClient.SendTextMessageAsync(update.Chat, response.Message);
            
        }
    }
}
