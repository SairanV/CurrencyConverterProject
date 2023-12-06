using System;
using Convert.DAL;
using Convert.BLL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Currency.Converter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repository = new Convert.DAL.CurrencyRateRepository();
            var rateService = new Convert.BLL.CurrencyRateService(repository);
            var converter = new Convert.BLL.CurrencyConverter(rateService);

            Console.WriteLine("Добро пожаловать в конвертер валют!");

            Console.Write("Введите исходную валюту: ");
            string sourceCurrency = Console.ReadLine().ToUpper(); 

            Console.Write("Введите целевую валюту: ");
            string targetCurrency = Console.ReadLine().ToUpper();

            Console.Write("Введите сумму для конвертации: ");
            if (decimal.TryParse(Console.ReadLine(), out decimal amount))
            {
                // Метод конвертации
                decimal convertedAmount = converter.ConvertCurrency(sourceCurrency, targetCurrency, amount);

                Console.WriteLine($"{amount} {sourceCurrency} равны {convertedAmount} {targetCurrency}");
            }
            else
            {
                Console.WriteLine("Некорректный ввод суммы.");
            }

            Console.ReadLine();
        }
    }
}
