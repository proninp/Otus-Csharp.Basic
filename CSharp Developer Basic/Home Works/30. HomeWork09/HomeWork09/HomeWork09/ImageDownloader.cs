using System;
using System.ComponentModel;
using System.Net;

namespace HomeWork09
{
    public class ImageDownloader
    {
        public event Action<string>? ImageStarted;
        public event Action<string>? ImageCompleted;
        private List<(Task task, string url, string filename)> downloadingInfoList;
        public ImageDownloader()
        {
            downloadingInfoList = new List<(Task task, string url, string filename)>();
        }

        public void StartDownload(Dictionary<string, string> ioResources)
        {
            foreach (var resource in ioResources)
                downloadingInfoList.Add((DownloadAsync(resource.Key, resource.Value), resource.Key, resource.Value));
        }
        public async Task DownloadAsync(string url, string filename)
        {
            var client = new WebClient();
            try
            {
                ImageStarted?.Invoke(filename);
                Helper.PrintLog($"Качаю \"{filename}\" из \"{url}\" .......\n\n");
                await client.DownloadFileTaskAsync(url, filename);
                ImageCompleted?.Invoke(filename);
                Helper.PrintLog($"Успешно скачал \"{filename}\" из \"{url}\"");
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
        public void GetDownloadingStatistics()
        {
            if (downloadingInfoList == null)
                return;
            foreach (var stat in downloadingInfoList)
            {
                if (stat.task.IsCompleted)
                    Helper.PrintLog($"{stat.filename} загружен", ConsoleColor.Magenta);
                else
                    Helper.PrintLog($"{stat.filename} не загружен", ConsoleColor.Magenta);
            }
        }
    }
}
