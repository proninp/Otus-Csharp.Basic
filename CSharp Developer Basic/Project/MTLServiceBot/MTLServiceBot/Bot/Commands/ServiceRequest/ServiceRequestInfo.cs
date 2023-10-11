using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands.ServiceRequest
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
                AssistLog.ColoredPrint(TextConsts.CBCmdDataEmpty, LogStatus.Error);
                return ("", command);
            }

            var data = callback.Data;
            var cbCmdPair = _callBackCommands.FirstOrDefault(cq => data.StartsWith(cq.Value));
            if (!data.Contains(TextConsts.CBCmdDataSeparator) || cbCmdPair.Equals(default(KeyValuePair<CallbackCommand, string>)))
            {
                AssistLog.ColoredPrint(string.Format(TextConsts.CBCmdDataNotRecognized, data), LogStatus.Error); // TODO Logging
                return ("", command);
            }

            command = cbCmdPair.Key;
            var dataList = data.Split(TextConsts.CBCmdDataSeparator);
            if (dataList.Length != 2)
            {
                AssistLog.ColoredPrint(string.Format(TextConsts.CBCmdDataUndefined, callback.Data), LogStatus.Error); // TODO Logging
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
                _ = botClient.SendTextMessageAsync(update.Chat, response.Message);
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
                AssistLog.ColoredPrint(ex.ToString(), LogStatus.Error); // TODO Logging
            }
            return serviceTask;
        }

        
    }
}
