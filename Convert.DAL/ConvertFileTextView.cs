using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Convert.DAL
{
    public class ConvertFileTextView
    {
        private string currentDirectory;
        private string logFilePath;

        public ConvertFileTextView(string initialDirectory, string logFileName)
        {
            currentDirectory = initialDirectory;
            logFilePath = Path.Combine(initialDirectory, logFileName);

            // Создаем лог-файл
            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Close();
            }
        }

        /// <summary>
        /// Метод для записи действия в лог-файл
        /// </summary>
        /// <param name="action"></param>
        private void LogAction(string action)
        {
            string logEntry = $"{DateTime.Now} - {action}";
            File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
        }

        /// <summary>
        /// Метод для отображения содержимого текущей директории
        /// </summary>
        public void ShowCurrentDirectoryContents(bool recursive = false)
        {
            try
            {
                DisplayDirectoryContents(currentDirectory, recursive);

                LogAction($"Просмотр {(recursive ? "рекурсивного " : "")}содержимого директории");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        /// <summary>
        /// Рекурсивный метод для отображения содержимого директории
        /// </summary>
        /// <param name="path"></param>
        /// <param name="recursive"></param>
        private void DisplayDirectoryContents(string path, bool recursive)
        {
            string[] files = Directory.GetFiles(path);
            string[] directories = Directory.GetDirectories(path);

            Console.WriteLine($"Содержимое директории {path}:");
            Console.WriteLine("Файлы:");
            foreach (string file in files)
            {
                Console.WriteLine(Path.GetFileName(file));
            }

            Console.WriteLine("\nДиректории:");
            foreach (string directory in directories)
            {
                Console.WriteLine(Path.GetFileName(directory));
                if (recursive)
                {
                    DisplayDirectoryContents(directory, true);
                }
            }
        }

        public void ReadFromFile(string fileName)
        {
            try
            {
                string filePath = Path.Combine(currentDirectory, fileName);

                if (File.Exists(filePath))
                {
                    string content = File.ReadAllText(filePath);
                    Console.WriteLine($"Содержимое файла {fileName}:\n{content}");

                    LogAction($"Чтение файла: {fileName}");
                }
                else
                {
                    Console.WriteLine("Файл не существует.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }


    }
}
