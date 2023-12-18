using System;
using System.IO;
using System.Reflection;
using Convert.DAL;
using Convert.BLL;
using System.Threading.Tasks;
using System.Configuration;

namespace Currency.Converter
{
    class Program
    {
        private static readonly string logFilePath = ConfigurationManager.AppSettings["LogFilePath"];
        [STAThread]
        static void Main(string[] args)
        {

            var repository = new CurrencyRateRepository();
            var rateService = new CurrencyRateService(repository);
            var converter = new CurrencyConverter(rateService);

            Console.WriteLine("Добро пожаловать в конвертер валют!");

            while (true)
            {
                Console.WriteLine("\nВыберите действие:");
                Console.WriteLine("1. Конвертация");
                Console.WriteLine("2. Показать курсы валют");
                Console.WriteLine("3. Показать историю конвертации");
                Console.WriteLine("4. Многовалютная конвертация");
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
                        case 4:
                            PerformMultiCurrencyConversion(rateService, converter);
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


        /// <summary>
        /// Выполняет конвертацию валюты и выводит результат на экран.
        /// </summary>
        /// <param name="rateService">Сервис для получения курсов валют.</param>
        /// <param name="converter">Конвертер валют.</param>
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

        /// <summary>
        /// Выводит на экран все курсы валют.
        /// </summary>
        /// <param name="rateRepository">Репозиторий для получения курсов валют.</param>
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


        /// <summary>
        /// Выводит на экран все курсы валют со всеми подробностями.
        /// </summary>
        /// <param name="rateRepository">Репозиторий для получения курсов валют.</param>
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


        /// <summary>
        /// Выводит на экран курс выбранной валюты.
        /// </summary>
        /// <param name="rateRepository">Репозиторий для получения курсов валют.</param>
        private static void DisplaySelectiveCurrencyRates(CurrencyRateRepository rateRepository)
        {
            Console.Write("Введите валюту для просмотра курса: ");
            string selectedCurrency = Console.ReadLine();

            selectedCurrency = selectedCurrency.ToUpper();

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

        /// <summary>
        /// Выводит на экран историю конвертации валют.
        /// </summary>
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

        /// <summary>
        /// Асинхронно обновляет курсы валют в фоновом режиме.
        /// </summary>
        /// <param name="rateService">Сервис для получения и обновления курсов валют.</param>
        /// <returns>Task</returns>
        private static async Task BackgroundUpdateRates(CurrencyRateService rateService)
        {
            while (true)
            {
                Console.WriteLine("\nАвтоматическое обновление курсов валют через 24 часа...");
                await Task.Delay(TimeSpan.FromHours(24));

                try
                {
                    rateService.UpdateCurrencyRates();
                    Console.WriteLine("Курсы валют успешно обновлены.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при автоматическом обновлении курсов валют: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Выполняет конвертацию исходной суммы в несколько разных валют.
        /// </summary>
        /// <param name="rateService">Сервис для получения курсов валют.</param>
        /// <param name="converter">Конвертер валют.</param>
        private static void PerformMultiCurrencyConversion(CurrencyRateService rateService, CurrencyConverter converter)
        {
            Console.Write("Введите исходную валюту: ");
            string sourceCurrency = Console.ReadLine();

            Console.Write("Введите целевые валюты через запятую (например, USD,EUR,GBP): ");
            string[] targetCurrencies = Console.ReadLine().Split(',');

            Console.Write("Введите сумму для конвертации: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                foreach (var targetCurrency in targetCurrencies)
                {
                    decimal exchangeRate = rateService.GetExchangeRate(sourceCurrency, targetCurrency);

                    if (exchangeRate != 0)
                    {
                        decimal convertedAmount = converter.ConvertCurrency(sourceCurrency, targetCurrency, amount);

                        Console.WriteLine($"Курс обмена {sourceCurrency} -> {targetCurrency}: {exchangeRate:F3}");
                        Console.WriteLine($"{amount} {sourceCurrency} равно {convertedAmount:F3} {targetCurrency}");
                    }
                    else
                    {
                        Console.WriteLine($"Конвертация в {targetCurrency} не выполнена из-за ошибки в получении курсов валют.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Некорректная сумма для конвертации.");
            }
        }

        /// <summary>
        /// Записывает информацию о конвертации в лог-файл.
        /// </summary>
        /// <param name="sourceCurrency">Исходная валюта.</param>
        /// <param name="targetCurrency">Целевая валюта.</param>
        /// <param name="amount">Исходная сумма.</param>
        /// <param name="convertedAmount">Сумма после конвертации.</param>
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
