using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

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

        private async Task HandleCallBackDataUpdate(ITelegramBotClient botClient, TgUpdate update, Session session,
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
                    if (await ChangeTaskStatus(botClient, update, session, serviceTask))
                        _ = UpdateSingleTaskInfo(botClient, update, session, serviceTask);
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

        private async Task<bool> ChangeTaskStatus(ITelegramBotClient botClient, TgUpdate update, Session session, ServiceTask task)
        {
            var response = await _api.ChangeServiceTaskStatus(session, task);
            if (!response.IsSuccess)
            {
                if (response.Status == ApiResponseStatus.Unauthorized)
                    _ = botClient.EditMessageReplyMarkupAsync(update.Chat.Id, update.Message.MessageId, replyMarkup: null);
                _ = botClient.SendTextMessageAsync(update.Chat, response.Message);
                return false;
            }
            return true;
        }

        private async Task<bool> UpdateSingleTaskInfo(ITelegramBotClient botClient, TgUpdate update, Session session, ServiceTask task)
        {
            // Делаем запрос для получения обновленной информации по сервисной задаче
            var serviceTask = await UpdateSingleServiceTaskInfo(session, task.Id);
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

        private async Task<ServiceTask?> UpdateSingleServiceTaskInfo(Session session, string taskId)
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

        private async void HandleAddNewFileCall(ITelegramBotClient botClient, TgUpdate update, Session session,
            List<ServiceTask> serviceTasksList, IReplyMarkup replyButtons)
        {
            var currentWorkFlowState = session.WorkFlowState;
            var taskId = session.WorkFlowTaskId;
            session.WorkFlowState = WorkFlow.ServiceRequests;
            if (currentWorkFlowState != WorkFlow.ServiceRequestAddFile || string.IsNullOrEmpty(taskId))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    TextConsts.CBCmdAddFileCallHandlerError,
                    parseMode: ParseMode.Html);
                return;
            }
            var serviceTask = serviceTasksList.FirstOrDefault(st => st.Id == taskId);

            if (serviceTask is null)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdServiceTaskNotFound, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

            if ((update.Message.Type is not (
                MessageType.Photo or
                MessageType.Video or
                MessageType.Document)) ||
                (update.Message.Photo is null &&
                update.Message.Video is null &&
                update.Message.Document is null))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdAddFileCallHandlerFileError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

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
            if (string.IsNullOrEmpty(fileId))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdAddFileCallHandlerFileIdError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }
            var fileInfo = await botClient.GetFileAsync(fileId);
            var filePath = fileInfo.FilePath;
        }
    }
}
