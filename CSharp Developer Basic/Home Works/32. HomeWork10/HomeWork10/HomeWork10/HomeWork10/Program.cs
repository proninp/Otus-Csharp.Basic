namespace HomeWork10
{
    internal class Program
    {
        private const string Directory1 = @"c:\Otus\TestDir1";
        private const string Directory2 = @"c:\Otus\TestDir2";
        static void Main(string[] args)
        {
            DirectoryInfo directoryInfo1 = new DirectoryInfo(Directory1);

            DirectoryInfo directoryInfo2 = new DirectoryInfo(Directory2);

            if (!directoryInfo1.Exists)
                directoryInfo1.Create();
            if (!directoryInfo2.Exists)
                directoryInfo2.Create();
            var filename = "SimpleFile";
            var fileExtension = ".txt";
            var fileCount = 10;
            string[] filesList = Enumerable.Range(1, fileCount)
                .Select(i => Path.Combine(directoryInfo1.FullName, $"{filename}{i}{fileExtension}"))
                .ToArray();

            for (int i = 0; i < fileCount; i++)
            {
                fullFilename = Path.Combine(directoryInfo1.FullName, $"{filename}{i + 1}{fileExtension}");
                if (File.Exists(fullFilename))
                    File.Delete(fullFilename);
                using(var fs = File.Create(fullFilename))
                {

                }
            }


        }
        
    }
}