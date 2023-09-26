using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Users;
using System.Runtime.Serialization;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public class GetServiceTasks : Command
    {
        public GetServiceTasks(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
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
                await botClient.SendTextMessageAsync(message.Chat, CaptionConstants.DeserializeJsonError);
                return;
            }
            if (serviceTasksList is null || serviceTasksList.Count == 0)
            {
                await botClient.SendTextMessageAsync(message.Chat, CaptionConstants.ServiceTasksListEmpty);
                return;
            }

            await botClient.SendTextMessageAsync(message.Chat,
                "Выберите сервисную заявку из списка",
                replyMarkup: GetServiceTasksButtons(serviceTasksList));
            //foreach(var task in serviceTasksList)
            //{
            //    await botClient.SendTextMessageAsync(message.Chat, task.ToString());
            //}


        }

        private IReplyMarkup GetServiceTasksButtons(List<ServiceTask> tasks)
        {
            var rows = new List<KeyboardButton[]>();
            var cols = new List<KeyboardButton>();
            for (int i = 0; i < tasks.Count; i++)
            {
                var kb = new KeyboardButton($"/st\t{tasks[i]}");
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
