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
    public partial class ServiceTasksRequest
    {
        private static readonly Dictionary<CallbackCommand, string> _callBackCommands = new()
        {
            { CallbackCommand.ChangeStatus,  TextConsts.CallBackCommandChangeStatus},
            { CallbackCommand.AddFile,  TextConsts.CallBackCommandAddFile}
        };

        private IReplyMarkup? GetServiceTaskInlineButtons(ServiceTask? task)
        {
            if (task is null || task.TaskStatus is not (ServiceTaskStatus.New or ServiceTaskStatus.Execution))
                return null;

            var inlineKeyboard = new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(task.GetNextStatusStepDescription(),
                    string.Format(_callBackCommands[CallbackCommand.ChangeStatus], task.Id)),
                InlineKeyboardButton.WithCallbackData(TextConsts.CallBackCommandAddFileDescription,
                    string.Format(_callBackCommands[CallbackCommand.AddFile], task.Id))
            };
            return new InlineKeyboardMarkup(inlineKeyboard);
        }
        private async Task HandleCallBackDataUpdate(ITelegramBotClient botClient, TgUpdate update, Session session,
            List<ServiceTask> serviceTasksList, IReplyMarkup replyButtons)
        {
            if (update.UpdateType is not UpdateType.CallbackQuery ||
                update.CallbackQuery is null || 
                string.IsNullOrEmpty(update.CallbackQuery.Data))
            {
                // TODO Log
                return;
            }
            var data = update.CallbackQuery.Data;
            var callBackCommand = _callBackCommands.FirstOrDefault(cq => data.StartsWith(cq.Value));
            if (callBackCommand.Equals(default(KeyValuePair<CallbackCommand, string>)) || !callBackCommand.Value.Contains(" "))
            {
                Program.ColoredPrint(string.Format(TextConsts.CallBackCommandDataNotRecognized, data), ConsoleColor.Red); // TODO Logging
                return;
            }
            
            var commandTaskId = callBackCommand.Value.Split(" ")[1];
            var task = serviceTasksList.FirstOrDefault(st => st.Id == commandTaskId);
            if (task is null)
            {
                _ = botClient.EditMessageTextAsync(update.Chat, update.Message.MessageId, update.Message.Text ?? "",
                    ParseMode.Html, replyMarkup: null);
                
                _ = botClient.SendTextMessageAsync(update.Chat, TextConsts.CallBackCommandServiceTaskNotFound,
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

            switch (callBackCommand.Key)
            {
                case CallbackCommand.ChangeStatus:
                    break;
                case CallbackCommand.AddFile:
                    break;
            }
        }

        private async Task ChangeTaskStatus(ITelegramBotClient botClient, TgUpdate update, Session session, ServiceTask task)
        {
            var response = await _api.ChangeServiceTaskStatus(session, task);
            if (!response.IsSuccess)
            {
                _ = botClient.SendTextMessageAsync(update.Chat, response.Message);
                return;
            }

            // Делаем запрос для получения обновленной информации по сервисной задаче
            var taskInfo = await GetSingleServiceTaskInfo(session, task.Id);
            // Выводим обновленную информацию в то же сообщение, обновляем кнопки
            
        }

        private async Task<string> GetSingleServiceTaskInfo(Session session, string taskId)
        {
            if (string.IsNullOrEmpty(taskId) || !taskId.Contains("_"))
                return "";
            var taskIdParts = taskId.Split("_");
            if (taskIdParts.Length != 2)
                return "";
            var response = await _api.GetServiceTask(session, taskIdParts[0], taskIdParts[1]);
            if (!response.IsSuccess)
                return "";
            ServiceTask? serviceTask;
            try
            {
                serviceTask = JsonSerializer.Deserialize<ServiceTask>(response.ResponseText);
            }
            catch (Exception ex)
            {
                Program.ColoredPrint(ex.ToString(), ConsoleColor.Red); // TODO Logging
                return "";
            }
            if (serviceTask is null)
                return "";
            return serviceTask.ToMarkedDownString();
        }

    }
}
