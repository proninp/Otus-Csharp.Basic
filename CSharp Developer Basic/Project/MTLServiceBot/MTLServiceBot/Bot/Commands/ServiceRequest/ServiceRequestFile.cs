using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.SQL;
using MTLServiceBot.Users;
using System.Net;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MTLServiceBot.Bot.Commands.ServiceRequest
{
    public partial class ServiceRequest
    {
        private void AddNewFileCall(ITelegramBotClient botClient, TgUpdate update, Session session, string taskId)
        {
            session.WorkFlowState = WorkFlow.ServiceRequestAddFile;
            session.WorkFlowTaskId = taskId;
            _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdAddFileCallMsgDescription, taskId),
                    parseMode: ParseMode.Html);
        }

        private async void HandleAddNewFileCallAsync(ITelegramBotClient botClient, TgUpdate update, Session session,
            List<ServiceTask> serviceTasksList, IReplyMarkup replyButtons)
        {
            if (!CheckAddNewFileCallParams(botClient, update, session, replyButtons, out string taskId))
                return;

            var serviceTask = serviceTasksList.FirstOrDefault(st => st.Id == taskId);
            if (serviceTask is null)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdServiceTaskNotFound, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

            string? fileId = GetAttachmentFileId(update);

            if (string.IsNullOrEmpty(fileId))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.AddFileHandleIdError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }
            var fileInfo = await botClient.GetFileAsync(fileId);
            if (fileInfo?.FilePath is null)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.AddFileHandleReceiveError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return;
            }

            var tgFilePath = fileInfo.FilePath;
            var destinationFilename = $"{serviceTask.RequestNo}-{serviceTask.TaskNo} {Path.GetFileName(tgFilePath)}";
            var localFilePath = Path.Combine(_tgFilesDirectory, update.From.Id.ToString(), destinationFilename);
            if (!await DownloadTgFileAsync(botClient, update, replyButtons, taskId, tgFilePath, localFilePath))
                return;

            var networkFileDirectory = Path.Combine(_sharedNetworkDirectory, update.From.Id.ToString());
            var copyResult = CopyFileToSharedNetworkDirectory(localFilePath, networkFileDirectory);
            if (!copyResult.isSuccess)
            {
                SendNotification(botClient, update.Chat, replyButtons,
                    string.Format(TextConsts.AddFileHandleCopyError, taskId),
                    LogStatus.Error, copyResult.exceptionText);
                return;
            }
            await AddNewTaskFileApiRequestAsync(botClient, update, session, serviceTask, destinationFilename, networkFileDirectory);
        }

        private bool CheckAddNewFileCallParams(ITelegramBotClient botClient, TgUpdate update, Session session,
            IReplyMarkup replyButtons, out string taskId)
        {
            taskId = session.WorkFlowTaskId;
            if (session.WorkFlowState != WorkFlow.ServiceRequestAddFile || string.IsNullOrEmpty(taskId))
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    TextConsts.CBCmdAddFileCallHandlerError,
                    parseMode: ParseMode.Html);
                return false;
            }

            if (!update.HasAttachment())
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.CBCmdAddFileCallHandlerFileError, taskId),
                    parseMode: ParseMode.Html,
                    replyMarkup: replyButtons);
                return false;
            }
            return true;
        }

        private string? GetAttachmentFileId(TgUpdate update)
        {
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
            return fileId;
        }

        private async Task<bool> DownloadTgFileAsync(ITelegramBotClient botClient, TgUpdate update, IReplyMarkup replyButtons,
            string taskId, string filePath, string destinationFilePath)
        {
            var dir = Path.GetDirectoryName(destinationFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (Stream fileStream = File.Create(destinationFilePath))
            {
                await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
            }
            if (!File.Exists(destinationFilePath))
            {
                SendNotification(botClient, update.Chat, replyButtons, string.Format(TextConsts.AddFileHandleReceiveError, taskId));
                return false;
            }
            return true;
        }

        private (bool isSuccess, string exceptionText) CopyFileToSharedNetworkDirectory(string localFilePath, string networkFileDirectory)
        {
            try
            {
                SetNetworkCredentials(networkFileDirectory);
                Directory.CreateDirectory(networkFileDirectory);
                File.Copy(localFilePath, Path.Combine(networkFileDirectory, Path.GetFileName(localFilePath)));
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        private async Task AddNewTaskFileApiRequestAsync(ITelegramBotClient botClient, TgUpdate update, Session session,
            ServiceTask task, string filename, string networkFileDirectory)
        {
            var response = await _api.AddNewFileToServiceTask(session, task, filename, Path.Combine(networkFileDirectory, filename));
            if (!response.IsSuccess)
            {
                SendNotification(botClient, update.Chat, string.Format(TextConsts.AddFileHandleCopyError, task.Id), LogStatus.Error, response.Message);
                return;
            }
            _ = botClient.SendTextMessageAsync(update.Chat,
                string.Format(TextConsts.AddFileHandleAddedMsg, task.Id, filename),
                parseMode: ParseMode.Html);
        }

        private void SetNetworkCredentials(string networkFileDirectory)
        {
            var networkCred = ConfigRepository.GetNetworkAccessCredentials();
            var login = networkCred.login;
            var pswCipher = EncryptionHelper.Decrypt(networkCred.pswCipher, login, login);

            var cred = new NetworkCredential(login, pswCipher);
            var credCache = new CredentialCache();
            credCache.Add(new Uri(networkFileDirectory), "Basic", cred);
        }
    }
}
