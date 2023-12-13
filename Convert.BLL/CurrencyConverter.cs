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

        public decimal ConvertCurrency(string sourceCurrency, string targetCurrency, decimal amount)
        {
            // Получаем курс обмена
            decimal exchangeRate = _rateService.GetExchangeRate(sourceCurrency, targetCurrency);

            // Выполняем конвертацию
            decimal convertedAmount = amount * exchangeRate;

            return convertedAmount;
        }
    }
}


