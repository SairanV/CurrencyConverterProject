using Convert.DAL;
using System;
using System.Reflection;

namespace Convert.BLL
{
    public class CurrencyRateService
    {
        private readonly CurrencyRateRepository _repository;

        public CurrencyRateService(CurrencyRateRepository repository)
        {
            _repository = repository;
        }

        public decimal GetExchangeRate(string sourceCurrency, string targetCurrency)
        {
            var rates = _repository.GetCurrencyRates();

            if (rates != null && rates.Rates != null)
            {
                double sourceRate, targetRate;

                if (!TryGetRate(rates.Rates, sourceCurrency, out sourceRate) ||
                    !TryGetRate(rates.Rates, targetCurrency, out targetRate))
                {
                    Console.WriteLine("Не удалось найти курс для одной из валют.");
                    return 0;
                }

                // В данном случае, предполагается, что курс обмена это отношение целевой валюты к исходной
                decimal exchangeRate = (decimal)targetRate / (decimal)sourceRate;

                return exchangeRate;
            }
            else
            {
                Console.WriteLine("Не удалось получить курсы валют.");
                return 0;
            }
        }

        private bool TryGetRate(Rates rates, string currency, out double rate)
        {
            rate = 0;

            PropertyInfo property = typeof(Rates).GetProperty(currency);
            if (property != null)
            {
                rate = (double)property.GetValue(rates);
                return true;
            }

            Console.WriteLine($"Не удалось найти курс для валюты {currency}");
            return false;
        }
    }
}