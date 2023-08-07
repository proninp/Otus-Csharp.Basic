namespace HomeWork09
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ImageDownloader imageDownloader = new();

            var start = () => Helper.PrintLog("Скачивание файла началось");
            var complete = () => Helper.PrintLog("Скачивание файла закончилось");
            imageDownloader.ImageStarted += start;
            imageDownloader.ImageCompleted += complete;

            var task = imageDownloader.DownloadAsync(Config.Url, Config.FileName);

            Helper.PrintLog("Нажмите клавишу A для выхода или любую другую клавишу для проверки статуса скачивания", ConsoleColor.Magenta);
            var isLoop = true;
            while (isLoop)
            {
                var c = Console.ReadKey();
                Console.WriteLine();
                switch (char.ToLower(c.KeyChar))
                {
                    case 'a': case 'а': // 'a' - латинская и 'а' - кириллица
                        isLoop = false;
                        break;
                    default:
                        if (task.IsCompleted)
                            Helper.PrintLog($"{Config.FileName} загружен", ConsoleColor.Magenta);
                        else
                            Helper.PrintLog($"{Config.FileName} не загружен", ConsoleColor.Magenta);
                        break;
                }
            }
            imageDownloader.ImageStarted -= start;
            imageDownloader.ImageCompleted -= complete;
        }
    }
}