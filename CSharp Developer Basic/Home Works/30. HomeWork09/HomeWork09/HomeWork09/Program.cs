using System;

namespace HomeWork09
{
    internal class Program
    {
        private static List<string> UrlList = new()
        {
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-07-15-norway-1-59704.jpg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-06-03-sedona-1-59297.jpeg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-05-20-a-lake-in-the-mountains-1-59172.jpeg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-05-16-cappadocia-turkey-1-59094.jpeg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-04-06-sunset-in-the-mountains-1-58663.jpeg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-04-04-aval-rock-1-58640.jpg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-04-04-yoho-national-park-1-58638.jpg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-03-29-autumn-carpathians-1-58550.jpeg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-03-07-anjihai-grand-canyon-1-58098.jpg",
            "https://wallscloud.net/img/resize/7680/4320/MM/2023-02-24-lake-louise-1-57854.jpg"
        };

        static void Main(string[] args)
        {
            PrepareIOResourcesMap(out Dictionary<string, string> map);

            ImageDownloader imageDownloader = new();

            var start = (string fileName) => Helper.PrintLog($"Скачивание файла {fileName} началось");
            var complete = (string fileName) => Helper.PrintLog($"Скачивание файла {fileName} закончилось");

            imageDownloader.ImageStarted += start;
            imageDownloader.ImageCompleted += complete;

            imageDownloader.StartDownload(map);

            Helper.PrintLog("Нажмите клавишу 'A' для выхода или любую другую клавишу для проверки статуса скачивания", ConsoleColor.Magenta);
            var isLoop = true;
            var endLoopChars = new Dictionary<char, bool>() { { 'a', true }, { 'а', true } }; // 'a' - латинская и 'а' - кириллица
            while (isLoop)
            {
                var c = char.ToLower(Console.ReadKey().KeyChar);
                Console.WriteLine();
                isLoop = !endLoopChars.GetValueOrDefault(c);
                if (isLoop)
                    imageDownloader.GetDownloadingStatistics();
            }
            imageDownloader.ImageStarted -= start;
            imageDownloader.ImageCompleted -= complete;
        }
        private static void PrepareIOResourcesMap(out Dictionary<string, string> map)
        {
            map = new Dictionary<string, string>();
            foreach(var url in UrlList)
            {
                string filename = url.Split("/").Last();
                map.Add(url, filename);
            }
        }
    }
}