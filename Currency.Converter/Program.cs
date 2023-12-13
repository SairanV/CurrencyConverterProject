using System;
using System.IO;
using System.Reflection;
using Convert.DAL;
using Convert.BLL;

namespace Currency.Converter
{
    class Program
    {
        private const string logFilePath = @"C:\Temp\log.txt";

        static void Main(string[] args)
        {
            var repository = new Convert.DAL.CurrencyRateRepository();
            var rateService = new Convert.BLL.CurrencyRateService(repository);
            var converter = new Convert.BLL.CurrencyConverter(rateService);

            Console.WriteLine("Добро пожаловать в конвертер валют!");

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Конвертация");
                Console.WriteLine("2. Показать курсы валют");
                Console.WriteLine("3. Показать историю конвертации");
                Console.WriteLine("0. Выход");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            PerformConversion(rateService, converter);
                            break;
                        case 2:
                            DisplayCurrencyRates(repository);
                            break;
                        case 3:
                            DisplayConversionHistory();
                            break;
                        case 0:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
                }
            }
        }

        private static void PerformConversion(CurrencyRateService rateService, CurrencyConverter converter)
        {
            Console.Write("Введите исходную валюту: ");
            string sourceCurrency = Console.ReadLine();

            Console.Write("Введите целевую валюту: ");
            string targetCurrency = Console.ReadLine();

            Console.Write("Введите сумму для конвертации: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                decimal exchangeRate = rateService.GetExchangeRate(sourceCurrency, targetCurrency);

                if (exchangeRate != 0)
                {
                    decimal convertedAmount = converter.ConvertCurrency(sourceCurrency, targetCurrency, amount);

                    // Ограничение до трех знаков после запятой
                    Console.WriteLine($"Курс обмена {sourceCurrency} -> {targetCurrency}: {exchangeRate:F3}");
                    Console.WriteLine($"{amount} {sourceCurrency} равно {convertedAmount:F3} {targetCurrency}");

                    // Запись в файл log.txt
                    LogConversion(sourceCurrency, targetCurrency, amount, convertedAmount);
                }
                else
                {
                    Console.WriteLine("Конвертация не выполнена из-за ошибки в получении курсов валют.");
                }
            }
            else
            {
                Console.WriteLine("Некорректная сумма для конвертации.");
            }
        }

        private static void DisplayCurrencyRates(CurrencyRateRepository rateRepository)
        {
            Console.WriteLine("Выберите опцию:\n1. Показать все курсы\n2. Показать выборочно");
            int option;
            if (int.TryParse(Console.ReadLine(), out option))
            {
                switch (option)
                {
                    case 1:
                        DisplayAllCurrencyRates(rateRepository);
                        break;
                    case 2:
                        DisplaySelectiveCurrencyRates(rateRepository);
                        break;
                    default:
                        Console.WriteLine("Некорректный выбор. Возвращение в основное меню.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Некорректный выбор. Возвращение в основное меню.");
            }
        }

        private static void DisplayAllCurrencyRates(CurrencyRateRepository rateRepository)
        {
            var rates = rateRepository.GetCurrencyRates();
            if (rates != null && rates.Rates != null)
            {
                Console.WriteLine("\nВсе курсы валют:");
                foreach (var property in typeof(Rates).GetProperties())
                {
                    Console.WriteLine($"{property.Name}: {property.GetValue(rates.Rates)}");
                }
            }
            else
            {
                Console.WriteLine("Не удалось получить курсы валют.");
            }
        }

        private static void DisplaySelectiveCurrencyRates(CurrencyRateRepository rateRepository)
        {
            Console.Write("Введите валюту для просмотра курса: ");
            string selectedCurrency = Console.ReadLine();

            var rates = rateRepository.GetCurrencyRates();
            if (rates != null && rates.Rates != null)
            {
                PropertyInfo property = typeof(Rates).GetProperty(selectedCurrency);
                if (property != null)
                {
                    Console.WriteLine($"\nКурс валюты {selectedCurrency}: {property.GetValue(rates.Rates)}");
                }
                else
                {
                    Console.WriteLine($"Не удалось найти курс для валюты {selectedCurrency}");
                }
            }
            else
            {
                Console.WriteLine("Не удалось получить курсы валют.");
            }
        }

        private static void DisplayConversionHistory()
        {
            Console.WriteLine("\nИстория конвертации:");

            try
            {
                string[] lines = File.ReadAllLines(logFilePath);
                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Файл истории конвертации не найден.");
            }
        }

        private static void LogConversion(string sourceCurrency, string targetCurrency, decimal amount, decimal convertedAmount)
        {
            string logEntry = $"{DateTime.Now} - Конвертация: {amount} {sourceCurrency} -> {convertedAmount} {targetCurrency}";

            try
            {
                File.AppendAllLines(logFilePath, new[] { logEntry });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи в файл log.txt: {ex.Message}");
            }
        }
    }
}
