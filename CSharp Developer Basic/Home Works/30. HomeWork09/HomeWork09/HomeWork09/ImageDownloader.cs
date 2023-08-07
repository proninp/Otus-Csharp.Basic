using System.Net;

namespace HomeWork09
{
    public class ImageDownloader
    {
        public event Action? ImageStarted;
        public event Action? ImageCompleted;


        public async Task DownloadAsync(string url, string fileName)
        {
            var webClient = new WebClient();
            ImageStarted?.Invoke();
            Helper.PrintLog($"Качаю \"{fileName}\" из \"{url}\" .......\n\n");
            try
            {
                await webClient.DownloadFileTaskAsync(url, fileName);

                ImageCompleted?.Invoke();
                Helper.PrintLog($"Успешно скачал \"{fileName}\" из \"{url}\"");

            }
            catch (ArgumentNullException argumentNullException)
            {
                Console.WriteLine($"Получил ошибку ArgumentNullException: {argumentNullException.Message}");
            }
            catch (WebException webException)
            {
                Console.WriteLine($"Получил ошибку WebException: {webException.Message}");
            }
            catch (NotSupportedException notSupportedException)
            {
                Console.WriteLine($"Получил ошибку NotSupportedException: {notSupportedException.Message}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Получил неопределенную ошибку: {exception.Message}");
            }
        }
        
    }
}
