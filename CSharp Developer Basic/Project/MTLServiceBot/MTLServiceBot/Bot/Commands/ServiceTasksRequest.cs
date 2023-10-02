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
    public partial class ServiceTasksRequest : Command
    {
        public ServiceTasksRequest(string name, string description, bool isRequireAuthentication): base(name, description, isRequireAuthentication)
        {
        }

        public override async Task HandleAsync(ITelegramBotClient botClient, Update update, Session session)
        {
            var message = update.Message!;
            var api = ServiceAPI.GetInstance();
            
            var response = await api.GetServiceTasks(session);
            if (!response.IsSuccess)
            {
                _ = botClient.SendTextMessageAsync(message.Chat, response.Message);
                return;
            }

            List<ServiceTask>? serviceTasksList = new();
            try
            {
                serviceTasksList = JsonSerializer.Deserialize<List<ServiceTask>>(response.ResponseText);
            }
            catch (Exception ex)
            {
                Program.ColoredPrint(ex.ToString(), ConsoleColor.Red); // TODO Logging
                _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.DeserializeJsonError, replyMarkup: new ReplyKeyboardRemove());
                return;
            }

            if (serviceTasksList is null || serviceTasksList.Count == 0)
            {
                _ = botClient.SendTextMessageAsync(message.Chat, TextConsts.ServiceTasksListEmpty);
                return;
            }
            
            if (message.Text == TextConsts.ServiceTasksCommandName) // Если запрос полного списка задач
                SendTasksMenuButtons(botClient, message, serviceTasksList);
            else
                SendSingleTaskInfo(botClient, message, serviceTasksList);
        }

        private void SendTasksMenuButtons(ITelegramBotClient botClient, Message message, List<ServiceTask> serviceTasksList)
        {
            WorkflowMode = true;
            botClient.SendTextMessageAsync(message.Chat,
                    TextConsts.ChooseServiceRequestBtnText,
                    replyMarkup: GetServiceTasksButtons(serviceTasksList));
        }

        private void SendSingleTaskInfo(ITelegramBotClient botClient, Message message, List<ServiceTask> serviceTasksList)
        {
            var numberFormat = GetNumberRequestParts(message.Text);
            if (!numberFormat.isValidNumberFormat)
            {
                botClient.SendTextMessageAsync(message.Chat, TextConsts.ServiceTasksWorkflowIncorrectFormat,
                    parseMode: ParseMode.Html,
                    replyMarkup: GetServiceTasksButtons(serviceTasksList));
                return;
            }

            var requestNo = numberFormat.numberParts[0];
            var taskNo = numberFormat.numberParts[1];
            if (!serviceTasksList.Any(st => st.RequestNo.Equals(requestNo) && st.TaskNo.Equals(taskNo)))
            {
                botClient.SendTextMessageAsync(message.Chat,
                    string.Format(TextConsts.ServiceTasksWorkflowNotFound, requestNo, taskNo, TextConsts.SingleTaskNumberFormatSeparator),
                    parseMode: ParseMode.Html,
                    replyMarkup: GetServiceTasksButtons(serviceTasksList));
                return;
            }

            var serviceTaskInfo = serviceTasksList.First(st => st.RequestNo.Equals(requestNo) && st.TaskNo.Equals(taskNo)).ToMarkedDownString();
            botClient.SendTextMessageAsync(message.Chat,
                    serviceTaskInfo,
                    parseMode: ParseMode.Html,
                    replyMarkup: GetServiceTasksButtons(serviceTasksList));
        }

        private (bool isValidNumberFormat, List<string> numberParts) GetNumberRequestParts(string message)
        {
            var reqParts = new List<string>();
            bool isValidPattern = message.Contains(TextConsts.SingleTaskNumberFormatSeparator);
            if (isValidPattern)
            {
                reqParts = message.Split(TextConsts.SingleTaskNumberFormatSeparator).ToList();
                isValidPattern = reqParts.Count == 2;
            }
            return (isValidPattern, reqParts);
        }

        private IReplyMarkup GetServiceTasksButtons(List<ServiceTask> tasks)
        {
            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            for (int i = 0; i < tasks.Count; i++)
            {
                var kb = new KeyboardButton(tasks[i].ToString());
                cols.Add(kb);
                if ((i + 1) % 2 != 0 && i != tasks.Count - 1)
                    continue;
                rows.Add(cols.ToArray());
                cols = new List<KeyboardButton>();
            }
            var replyKB = new ReplyKeyboardMarkup(rows.ToArray());
            replyKB.ResizeKeyboard = true;
            return replyKB;
        }
    }
}
