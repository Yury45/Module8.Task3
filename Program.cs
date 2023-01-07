namespace Module8.Task3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ClearDirWithInfo(@"C:\Downloads\123");
        }

        static void ClearDirWithInfo(string path)
        {
            var info = ClearFolders(path);
            Console.WriteLine($"Размер до очистки: {info.Item3}");
            Console.WriteLine($"Удалено файлов: {info.Item1}");
            Console.WriteLine($"Удалено каталогов:{info.Item2}");
            Console.WriteLine($"Общий объем директории после очистки: {GetDirSize(path)}");
        }

        static (int, int, double) ClearFolders(string path)
        {
            int counterFiles = 0;
            int counterDirs = 0;
            double sizeDir = 0;

            try
            {
                DirectoryInfo directory = new DirectoryInfo(path);

                if (Directory.Exists(path))
                {
                    foreach (var dir in directory.GetDirectories())
                    {

                        if ((DateTime.Now - Directory.GetLastAccessTime(dir.FullName)).TotalMinutes > 30)
                        {
                            var tuple = ClearFolders(dir.FullName);
                            counterFiles += tuple.Item1;
                            counterDirs += tuple.Item2;
                            sizeDir += tuple.Item3;
                            Directory.Delete(dir.FullName, true);
                            counterDirs++;
                        }
                        else
                        {
                            var tuple = ClearFolders(dir.FullName);
                            counterFiles += tuple.Item1;
                            counterDirs += tuple.Item2;
                            sizeDir += tuple.Item3;
                        }
                    }
                    foreach (var file in directory.GetFiles())
                    {
                        if ((DateTime.Now - Directory.GetLastAccessTime(file.FullName)).TotalMinutes > 30)
                        {
                            sizeDir += file.Length;
                            File.Delete(file.FullName);
                            counterFiles++;
                        }
                        else
                        {
                            sizeDir += file.Length;
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Директория {path} отсутствует.");
                }

                return (counterFiles, counterDirs, sizeDir);

            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"Директория не обнаружена! {e.Message}");
                return (counterFiles, counterDirs, sizeDir);
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Отсутствует доступ! {e.Message}");
                return (counterFiles, counterDirs, sizeDir);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Возникла неопознанная ошибка ошибка: {e.Message}");
                return (counterFiles, counterDirs, sizeDir);
            }
        }

        static double GetDirSize(string path)
        {
            double sizeFile = 0;
            try
            {
                if (Directory.Exists(path))
                {
                    DirectoryInfo directory = new DirectoryInfo(path);
                    var dirs = directory.GetDirectories();

                    foreach (var dir in dirs)
                    {
                        sizeFile += GetDirSize(dir.FullName);
                    }

                    foreach (var file in directory.GetFiles())
                    {
                        sizeFile += file.Length;
                    }
                }
                else
                {
                    Console.WriteLine($"Директория {path} отсутствует.");
                }

                return sizeFile;
            }
            catch (DirectoryNotFoundException e)
            {
                Console.WriteLine($"Директория не обнаружена! {e.Message}");
                return 0;
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine($"Отсутствует доступ! {e.Message}");
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Возникла неопознанная ошибка ошибка: {e.Message}");
                return 0;
            }
        }
    }

}