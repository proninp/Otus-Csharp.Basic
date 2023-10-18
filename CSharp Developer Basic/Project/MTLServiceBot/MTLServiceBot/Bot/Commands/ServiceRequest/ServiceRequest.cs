using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.SQL;
using MTLServiceBot.Users;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands.ServiceRequest
{
    public partial class ServiceRequest : Command
    {
        private readonly string _tgFilesDirectory;
        private readonly string _sharedNetworkDirectory;

        public ServiceRequest(string name, string description, bool isRequireAuthentication) : base(name, description, isRequireAuthentication)
        {
            _tgFilesDirectory = ConfigRepository.GetDownloadedFilesDirectory();
            _sharedNetworkDirectory = ConfigRepository.GetSharedNetworkDirectory();
        }

        public override async Task HandleAsync(ITelegramBotClient botClient, TgUpdate update, Session session)
        {
            var api = new ServiceAPI(session);
            var apiResponse = await api.GetServiceTasksAsync();
            if (!apiResponse.IsSuccess)
            {
                SendNotification(botClient, update.Chat, new ReplyKeyboardRemove(), apiResponse.Message, LogStatus.Warning);
                return;
            }

            if (!GetServiceTasksList(out List<ServiceTask>? serviceTasksList, botClient, update, apiResponse.ResponseText))
                return;

            var replyButtons = GetServiceTasksReplyButtons(serviceTasksList!);

            if (!string.IsNullOrEmpty(update.Text) && update.Text.Equals(TextConsts.ServiceTasksCommandName)) // Если запрос полного списка задач
            {
                session.WorkFlowState = WorkFlow.ServiceRequests;
                SendMenuButtons(botClient, update.Message, replyButtons);
                return;
            }

            CheckForServiceWorkFlowStepChange(update, session);

            switch (update.UpdateType)
            {
                case UpdateType.Message:
                    if (session.WorkFlowState == WorkFlow.ServiceRequests)
                        SendSingleTaskInfo(botClient, update.Message, serviceTasksList!, replyButtons);
                    else if (session.WorkFlowState == WorkFlow.ServiceRequestAddFile)
                        HandleAddNewFileCallAsync(botClient, update, session, serviceTasksList!, replyButtons);
                    break;
                case UpdateType.CallbackQuery:
                    _ = HandleCallBackDataUpdateAsync(botClient, update, session, serviceTasksList!, replyButtons);
                    break;
            }
        }

        private bool GetServiceTasksList(out List<ServiceTask>? serviceTasksList, ITelegramBotClient botClient, TgUpdate update, string apiResponseText)
        {
            serviceTasksList = new();
            try
            {
                serviceTasksList = JsonSerializer.Deserialize<List<ServiceTask>>(apiResponseText);
            }
            catch (Exception ex)
            {
                SendNotification(botClient, update.Chat, new ReplyKeyboardRemove(), TextConsts.DeserializeJsonError, LogStatus.Error, ex.ToString());
                return false;
            }

            if (serviceTasksList is null || serviceTasksList.Count == 0)
            {
                SendNotification(botClient, update.Chat, new ReplyKeyboardRemove(), TextConsts.DeserializeJsonError, LogStatus.Warning);
                return false;
            }
            return true;
        }

        private void SendMenuButtons(ITelegramBotClient botClient, Message message, IReplyMarkup replyButtons) =>
            botClient.SendTextMessageAsync(message.Chat, TextConsts.ChooseServiceRequestBtn, replyMarkup: replyButtons);

        private void SendSingleTaskInfo(ITelegramBotClient botClient, Message message, List<ServiceTask> serviceTasksList, IReplyMarkup replyButtons)
        {
            var numberFormat = GetNumberRequestParts(message.Text ?? "");
            if (!numberFormat.isValidNumberFormat)
            {
                SendNotification(botClient, message.Chat, GetServiceTasksReplyButtons(serviceTasksList),
                    TextConsts.ServiceTasksWorkflowIncorrectFormat, LogStatus.Warning);
                return;
            }

            var requestNo = numberFormat.numberParts[0];
            var taskNo = numberFormat.numberParts[1];
            if (!serviceTasksList.Any(st => st.RequestNo.Equals(requestNo) && st.TaskNo.Equals(taskNo)))
            {
                SendNotification(botClient, message.Chat, GetServiceTasksReplyButtons(serviceTasksList),
                    string.Format(TextConsts.ServiceTasksWorkflowNotFound, requestNo, taskNo, TextConsts.SingleTaskNumberFormatSeparator),
                    LogStatus.Warning);
                return;
            }

            var serviceTask = serviceTasksList.First(st => st.RequestNo.Equals(requestNo) && st.TaskNo.Equals(taskNo));
            var inlineButtons = GetServiceTaskInlineButtons(serviceTask);
            var serviceTaskInfo = serviceTask.ToMarkedDownString();

            botClient.SendTextMessageAsync(message.Chat,
                    serviceTask.ToMarkedDownString(),
                    parseMode: ParseMode.Html,
                    replyMarkup: inlineButtons ?? replyButtons);
        }

        private (bool isValidNumberFormat, List<string> numberParts) GetNumberRequestParts(string messageText)
        {
            var reqParts = new List<string>();
            bool isValidPattern = messageText.Contains(TextConsts.SingleTaskNumberFormatSeparator);
            if (isValidPattern)
            {
                reqParts = messageText.Split(TextConsts.SingleTaskNumberFormatSeparator).ToList();
                isValidPattern = reqParts.Count == 2;
            }
            return (isValidPattern, reqParts);
        }

        private IReplyMarkup GetServiceTasksReplyButtons(List<ServiceTask> tasks)
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

        private void CheckForServiceWorkFlowStepChange(TgUpdate update, Session session)
        {
            if (session.WorkFlowState == WorkFlow.ServiceRequestAddFile)
            {
                if (update.UpdateType == UpdateType.CallbackQuery ||
                    (!update.HasAttachment() &&
                    !string.IsNullOrEmpty(update.Message.Text)))
                {
                    session.WorkFlowState = WorkFlow.ServiceRequests;
                    session.WorkFlowTaskId = "";
                }
            }
        }
    }
}
