using System;
using Convert.DAL;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convert.BLL
{
    public class CurrencyConverter
    {
        private readonly CurrencyRateService _rateService;

        public CurrencyConverter(CurrencyRateService rateService)
        {
            _rateService = rateService;
        }

        /// <summary>
        /// Выполняет конвертацию суммы из одной валюты в другую.
        /// </summary>
        /// <param name="sourceCurrency">Исходная валюта.</param>
        /// <param name="targetCurrency">Целевая валюта.</param>
        /// <param name="amount">Сумма для конвертации.</param>
        /// <returns>Сумма в целевой валюте после конвертации.</returns>
        public decimal ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            sourceCurrency = sourceCurrency.ToUpper();
            targetCurrency = targetCurrency.ToUpper();

            // Получаем курс обмена
            decimal exchangeRate = _rateService.GetExchangeRate(sourceCurrency, targetCurrency);

            // Выполняем конвертацию
            decimal convertedAmount = amount * exchangeRate;

            return convertedAmount;
        }
    }
}


