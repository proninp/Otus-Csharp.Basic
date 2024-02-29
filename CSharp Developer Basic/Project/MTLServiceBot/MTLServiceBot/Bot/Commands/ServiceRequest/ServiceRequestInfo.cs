using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.Users;
using Serilog;
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
            { CallbackCommand.ChangeStatus, AppConfig.Instance.CBCmdChangeStatus},
            { CallbackCommand.AddFile, AppConfig.Instance.CBCmdAddFile}
        };

        private InlineKeyboardMarkup? GetServiceTaskInlineButtons(ServiceTask? task)
        {
            if (task is null || task.TaskStatus is not (ServiceTaskStatus.New or ServiceTaskStatus.Execution))
                return null;

            var inlineKeyboard = new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(task.GetNextStatusStepDescription(),
                    $"{_callBackCommands[CallbackCommand.ChangeStatus]}{AppConfig.Instance.CBCmdDataSeparator}{task.Id}"),
                InlineKeyboardButton.WithCallbackData(AppConfig.Instance.CBCmdAddFileDescription,
                $"{_callBackCommands[CallbackCommand.AddFile]}{AppConfig.Instance.CBCmdDataSeparator}{task.Id}")
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
                SendNotification(botClient, update.Chat, replyButtons, string.Format(AppConfig.Instance.CBCmdServiceTaskNotFound, cmdTaskId),
                    Serilog.Events.LogEventLevel.Warning);
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
                Log.Warning(AppConfig.Instance.CBCmdDataEmpty);
                return ("", command);
            }

            var data = callback.Data;
            var cbCmdPair = _callBackCommands.FirstOrDefault(cq => data.StartsWith(cq.Value));
            if (!data.Contains(AppConfig.Instance.CBCmdDataSeparator) || cbCmdPair.Equals(default(KeyValuePair<CallbackCommand, string>)))
            {
                Log.Warning(string.Format(AppConfig.Instance.CBCmdDataNotRecognized, data));
                return ("", command);
            }

            command = cbCmdPair.Key;
            var dataList = data.Split(AppConfig.Instance.CBCmdDataSeparator);
            if (dataList.Length != 2)
            {
                Log.Warning(string.Format(AppConfig.Instance.CBCmdDataUndefined, callback.Data));
                return ("", command);
            }
            return (dataList[1], command);
        }

        private async Task<bool> ChangeTaskStatusApiRequestAsync(ITelegramBotClient botClient, TgUpdate update,
            Session session, ServiceTask serviceTask)
        {
            var api = new ServiceAPI(session);
            var response = await api.ChangeServiceTaskStatusAsync(serviceTask);
            if (!response.IsSuccess)
            {
                if (response.Status == ApiResponseStatus.Unauthorized)
                    _ = botClient.EditMessageReplyMarkupAsync(update.Chat.Id, update.Message.MessageId, replyMarkup: null);
                SendNotification(botClient, update.Chat, response.Message, Serilog.Events.LogEventLevel.Warning);
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
                SendNotification(botClient, update.Chat,
                    string.Format(AppConfig.Instance.SingleServiceRequestUpdateFailureMsg, task.RequestNo, task.TaskNo),
                    Serilog.Events.LogEventLevel.Warning);
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
            if (string.IsNullOrEmpty(taskId) || !taskId.Contains(AppConfig.Instance.SingleTaskNumberFormatSeparator))
                return serviceTask;
            var taskIdParts = taskId.Split(AppConfig.Instance.SingleTaskNumberFormatSeparator);
            if (taskIdParts.Length != 2)
                return serviceTask;
            var api = new ServiceAPI(session);
            var response = await api.GetServiceTasksAsync(taskIdParts[0], taskIdParts[1]);
            if (!response.IsSuccess)
                return serviceTask;

            try
            {
                serviceTask = JsonSerializer.Deserialize<List<ServiceTask>>(response.ResponseText)?.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
            return serviceTask;
        }
    }
}
