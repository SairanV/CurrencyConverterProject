using Convert.DAL;


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

            decimal exchangeRate = 0; 

            return exchangeRate;
        }
    }
}

