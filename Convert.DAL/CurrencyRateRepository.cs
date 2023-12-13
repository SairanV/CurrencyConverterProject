using RestSharp;
using Newtonsoft.Json;
using Convert.DAL;

namespace Convert.DAL
{
    using Newtonsoft.Json;
    using RestSharp;

    public class CurrencyRateRepository
    {
        private readonly string apiUrl = "https://www.cbr-xml-daily.ru/";

        public Root GetCurrencyRates()
        {
            var client = new RestClient(apiUrl);
            var request = new RestRequest("latest.js", Method.Get);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                var rr = JsonConvert.DeserializeObject<Root>(response.Content);
                return rr;
            }
            else
            {
                return null;
            }
        }
    }

}