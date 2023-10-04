using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands
{
    public partial class ServiceTasksRequest
    {
        private IReplyMarkup? GetServiceTaskInlineButtons(ServiceTask? task)
        {
            if (task is null || task.TaskStatus is not (ServiceTaskStatus.New or ServiceTaskStatus.Execution))
                return null;

            var inlineKeyboard = new InlineKeyboardButton[]
            {
                InlineKeyboardButton.WithCallbackData(task.GetNextStatusDescription(),
                    string.Format(TextConsts.CallBackCommandChangeStatus, task.Id)),
            InlineKeyboardButton.WithCallbackData(TextConsts.CallBackCommandAddFileDescription,
                string.Format(TextConsts.CallBackCommandAddFile, task.Id))
            };
            return new InlineKeyboardMarkup(inlineKeyboard);
        }
    }
}
