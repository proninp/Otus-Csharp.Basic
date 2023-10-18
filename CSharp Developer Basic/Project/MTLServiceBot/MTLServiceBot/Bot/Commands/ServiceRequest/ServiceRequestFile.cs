using MTLServiceBot.API;
using MTLServiceBot.API.Entities;
using MTLServiceBot.Assistants;
using MTLServiceBot.SQL;
using MTLServiceBot.Users;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
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

            ApiResponse apiResponse;
            var api = new ServiceAPI(session);

            if (ConfigRepository.GetSendAsFileSetup())
            {
                using (var fileStream = await DownloadFileAsync(botClient, tgFilePath))
                {
                    apiResponse = await api.AddNewFileToServiceTaskAsync(serviceTask, fileStream, destinationFilename);
                }
            }
            else
            {
                var networkFileDirectory = await UploadFileToNetworkPathAsync(botClient, update, replyButtons, taskId, tgFilePath, destinationFilename);
                if (string.IsNullOrEmpty(networkFileDirectory))
                    return;
                apiResponse = await api.AddNewFileToServiceTaskAsync(serviceTask, Path.Combine(networkFileDirectory, destinationFilename), 
                    destinationFilename);
            }
            
            if (apiResponse.IsSuccess)
            {
                _ = botClient.SendTextMessageAsync(update.Chat,
                    string.Format(TextConsts.AddFileHandleAddedMsg, serviceTask.Id, destinationFilename),
                    parseMode: ParseMode.Html);
            }
            else
            {
                SendNotification(botClient, update.Chat, string.Format(TextConsts.AddFileHandleCopyError,
                    serviceTask.Id), LogStatus.Error, apiResponse.Message);
            }
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

        private async Task<Stream> DownloadFileAsync(ITelegramBotClient botClient, string filePath)
        {
            var fileStream = new MemoryStream();
            await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
            fileStream.Seek(0, SeekOrigin.Begin);
            return fileStream;
        }

        private async Task<string> UploadFileToNetworkPathAsync(ITelegramBotClient botClient, TgUpdate update, IReplyMarkup replyButtons, string taskId,
            string tgFilePath, string destinationFilename)
        {
            var localFilePath = Path.Combine(_tgFilesDirectory, update.From.Id.ToString(), destinationFilename);

            if (!await DownloadFileAsyncAsync(botClient, update, replyButtons, taskId, tgFilePath, localFilePath))
                return "";

            var networkFileDirectory = Path.Combine(_sharedNetworkDirectory, update.From.Id.ToString());
            if (!CopyFileToSharedNetworkDirectory(botClient, update, replyButtons, taskId, localFilePath, networkFileDirectory))
                return "";

            return networkFileDirectory;
        }

        private async Task<bool> DownloadFileAsyncAsync(ITelegramBotClient botClient, TgUpdate update, IReplyMarkup replyButtons,
            string taskId, string filePath, string destinationFilePath)
        {
            var dir = Path.GetDirectoryName(destinationFilePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            using (Stream fileStream = System.IO.File.Create(destinationFilePath))
            {
                await botClient.DownloadFileAsync(filePath: filePath, destination: fileStream);
            }
            if (!System.IO.File.Exists(destinationFilePath))
            {
                SendNotification(botClient, update.Chat, replyButtons, string.Format(TextConsts.AddFileHandleReceiveError, taskId));
                return false;
            }
            return true;
        }

        private bool CopyFileToSharedNetworkDirectory(ITelegramBotClient botClient, TgUpdate update, IReplyMarkup replyButtons, 
            string taskId, string localFilePath, string networkFileDirectory)
        {
            try
            {
                SetNetworkCredentials(networkFileDirectory);
                Directory.CreateDirectory(networkFileDirectory);
                System.IO.File.Copy(localFilePath, Path.Combine(networkFileDirectory, Path.GetFileName(localFilePath)));
                return true;
            }
            catch (Exception ex)
            {
                SendNotification(botClient, update.Chat, replyButtons,
                    string.Format(TextConsts.AddFileHandleCopyError, taskId),
                    LogStatus.Error, ex.Message);
                return false;
            }
        }

        private void SetNetworkCredentials(string networkFileDirectory)
        {
            var access = ConfigRepository.GetNetworkAccessCredentials();
            var login = access.login;
            var pswCipher = EncryptionHelper.Decrypt(access.pswCipher, login, login);

            var cred = new NetworkCredential(login, pswCipher);
            var credCache = new CredentialCache();
            credCache.Add(new Uri(networkFileDirectory), "Basic", cred);
        }
    }
}
