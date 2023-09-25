using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Users;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;

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
            var response = await api.GetServiceTasksList(session);
            if (response.Status == ApiResponseStatus.Unauthorized)
            {
                if (!await base.ReAuthentication(botClient, message, session))
                    return;
                response = await api.GetServiceTasksList(session);
            }
            if (!await CheckIsValidResponse(response, botClient, message.Chat))
                return;
            
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
        }
        private async Task<bool> CheckIsValidResponse(ApiResponse response, ITelegramBotClient botClient, Chat chat)
        {
            if (response.Status == ApiResponseStatus.Unauthorized)
            {
                await botClient.SendTextMessageAsync(chat, CaptionConstants.UnauthorizedError);
                return false;
            }
            if (response.Status == ApiResponseStatus.Error)
            {
                await botClient.SendTextMessageAsync(chat, CaptionConstants.ServerConnectionError);
                return false;
            }
            if (string.IsNullOrEmpty(response.ResponseText))
            {
                await botClient.SendTextMessageAsync(chat, CaptionConstants.ServiceTasksListEmpty);
                return false;
            }
            return true;
        }
    }
}
