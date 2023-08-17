using System.IO;
using System.Text;

namespace HomeWork10
{
    internal class Program
    {
        private static readonly string Directory1 = Path.Combine("c:", "Otus", "TestDir1");
        private static readonly string Directory2 = Path.Combine("c:", "Otus", "TestDir2");
        private static IEnumerable<string> FilesList = Enumerable.Range(1, 10).Select(i => $"Simplefile{i.ToString().PadLeft(2, '0')}.txt");

        static void Main(string[] args)
        {
            CreateDirectiry(Directory1);
            CreateDirectiry(Directory2);

            CreateFiles(Directory1, FilesList);
            CreateFiles(Directory2, FilesList);

            AppendText(Directory1, FilesList);
            AppendText(Directory2, FilesList);

            PrintFiles(Directory1, FilesList);
            PrintFiles(Directory2, FilesList);
        }
        public static void CreateDirectiry(string dir)
        {
            try
            {
                var directoryInfo = new DirectoryInfo(dir);

                if (!directoryInfo.Exists)
                    directoryInfo.Create();
            }
            catch(Exception ex)
            {
                Helper.ConsolePrint(ex.Message, ConsoleColor.DarkRed);
            }
        }
        public static void CreateFiles(string directory, IEnumerable<string> files)
        {
            try
            {
                foreach(var filename in files)
                {
                    using var streamWriter = File.CreateText(Path.Combine(directory, filename), );
                    streamWriter.WriteLine(filename);
                }
            }
            catch(Exception ex)
            {
                Helper.ConsolePrint(ex.Message, ConsoleColor.DarkRed);
            }
        }
        public static void AppendText(string directory, IEnumerable<string> files)
        {
            try
            {
                foreach(var filename in files)
                {
                    using var streamWriter = File.AppendText(Path.Combine(directory, filename));
                    streamWriter.WriteLine(DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                Helper.ConsolePrint(ex.Message, ConsoleColor.DarkRed);
            }
        }
        public static void PrintFiles(string directory, IEnumerable<string> files)
        {
            try
            {
                foreach(var filename in files)
                {
                    string fullFilename = Path.Combine(directory, filename);
                    using var streamReader = File.OpenText(fullFilename);
                    PrintResult(fullFilename, streamReader.ReadToEnd());
                }
            }
            catch(Exception ex)
            {
                Helper.ConsolePrint(ex.Message, ConsoleColor.DarkRed);
            }
        }
        private static void PrintResult(string fullFilename, string content)
        {
            Console.Write($"Full filename: ");
            Helper.ConsolePrint($"{fullFilename}; ", false, ConsoleColor.Magenta);
            Console.WriteLine("With content:");
            Helper.ConsolePrint(content, true);
            Console.WriteLine();
        }
        
    }
};