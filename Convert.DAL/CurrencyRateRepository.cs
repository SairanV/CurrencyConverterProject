using System;
using System.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace Convert.DAL
{
    public class CurrencyRateRepository
    {
        private readonly string apiUrl = ConfigurationManager.AppSettings["ApiUrl"];
        private Root cachedRates;
        private DateTime lastUpdateTime;

        /// <summary>
        /// Получает текущие курсы валют.
        /// </summary>
        /// <returns>Объект, содержащий информацию о курсах валют.</returns>
        /// <exception cref="Exception">Возникает при ошибке при получении курсов валют.</exception>
        public Root GetCurrencyRates()
        {
            // Проверка, нужно ли обновить курсы валют (раз в день)
            if (cachedRates == null || (DateTime.Now - lastUpdateTime).Days >= 1)
            {
                try
                {
                    var client = new RestClient(apiUrl);
                    var request = new RestRequest("latest.js", Method.Get);

                    var response = client.Execute(request);

                    if (response.IsSuccessful)
                    {
                        cachedRates = JsonConvert.DeserializeObject<Root>(response.Content);
                        lastUpdateTime = DateTime.Now;
                    }
                    else
                    {
                        throw new Exception($"Ошибка при получении курсов валют. Код ошибки: {response.StatusCode}, Сообщение: {response.ErrorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Ошибка при получении курсов валют: {ex.Message}");
                }
            }

            return cachedRates;
        }

        /// <summary>
        /// Обновляет текущие курсы валют.
        /// </summary>
        /// <exception cref="Exception">Возникает при ошибке при обновлении курсов валют.</exception>
        public void UpdateCurrencyRates()
        {
            try
            {
                var client = new RestClient(apiUrl);
                var request = new RestRequest("latest.js", Method.Get);

                var response = client.Execute(request);

                if (response.IsSuccessful)
                {
                    cachedRates = JsonConvert.DeserializeObject<Root>(response.Content);
                    lastUpdateTime = DateTime.Now;
                }
                else
                {
                    throw new Exception($"Ошибка при обновлении курсов валют. Код ошибки: {response.StatusCode}, Сообщение: {response.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка при обновлении курсов валют: {ex.Message}");
            }
        }
    }
}
