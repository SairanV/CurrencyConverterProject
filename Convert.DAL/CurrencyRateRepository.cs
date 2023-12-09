using RestSharp;
using Newtonsoft.Json;

namespace Convert.DAL
{
    public class CurrencyRateRepository
    {
        private readonly string apiUrl = "https://www.cbr-xml-daily.ru/";

        public string  GetCurrencyRates()
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest("latest.js", Method.Get);  

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {

                var rr = JsonConvert.DeserializeObject<Root>(response.Content);

                return response.Content;
            }
            else
            {
                return null;
            }
        }
    }
}
