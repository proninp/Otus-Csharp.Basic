using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Users;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class SerciceTasksRequest : Command
    {
        public SerciceTasksRequest(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
        }

        public override async Task Handle(ITelegramBotClient botClient, Message message, Session session)
        {
            var api = ServiceAPI.GetInstance();
            var response = await api.GetServiceTasks(session);
            if (!response.IsSuccess)
            {
                await botClient.SendTextMessageAsync(message.Chat, response.Message);
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
                await botClient.SendTextMessageAsync(message.Chat, TextConsts.DeserializeJsonError);
                return;
            }

            if (serviceTasksList is null || serviceTasksList.Count == 0)
            {
                await botClient.SendTextMessageAsync(message.Chat, TextConsts.ServiceTasksListEmpty);
                return;
            }

            if (message.Text == TextConsts.ServiceTasksCommandName) // Если запрос полного списка задач
            {
                await botClient.SendTextMessageAsync(message.Chat,
                    TextConsts.ChooseServiceRequestBtnText,
                    replyMarkup: GetServiceTasksButtons(serviceTasksList));
                WorkflowMode = true;
                return;
            }

            await GetSingleTaskInfo(botClient, message, serviceTasksList);
        }

        private async Task GetSingleTaskInfo(ITelegramBotClient botClient, Message message, List<ServiceTask> serviceTasksList)
        {
            var numberFormat = GetNumberRequestParts(message.Text);
            if (!numberFormat.isValidNumberFormat)
            {
                await botClient.SendTextMessageAsync(message.Chat, TextConsts.ServiceTasksWorkflowIncorrectFormat, parseMode: ParseMode.Markdown);
                return;
            }
            var requestNo = numberFormat.numberParts[0];
            var taskNo = numberFormat.numberParts[1];
            if (!serviceTasksList.Any(st => st.RequestNo.Equals(requestNo) && st.TaskNo.Equals(taskNo)))
            {
                await botClient.SendTextMessageAsync(message.Chat,
                    string.Format(TextConsts.ServiceTasksWorkflowNotFound, requestNo, taskNo, TextConsts.SingleTaskNumberFormatSeparator),
                    parseMode: ParseMode.Markdown);
                return;
            }
            await botClient.SendTextMessageAsync(message.Chat,
                    serviceTasksList.First(st => st.RequestNo.Equals(requestNo) && st.TaskNo.Equals(taskNo)).ToMarkedDownString(),
                    parseMode: ParseMode.Markdown);
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
