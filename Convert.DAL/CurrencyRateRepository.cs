using RestSharp;

namespace Convert.DAL
{
    public class CurrencyRateRepository
    {
        private readonly string apiUrl = "https://api.bcc.kz:10443/bcc/production/v1/public/rate";

        public CurrencyRateResponse GetCurrencyRates()
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest("your_endpoint", Method.Get);  // я не знаю что здесь писать "your_endpoint"

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return response.Content;  //ошибка
            }
            else
            {
                return null;
            }
        }
    }
}
