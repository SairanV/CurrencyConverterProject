using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Convert.DAL
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Rates
    {
        public double AUD { get; set; }
        public double AZN { get; set; }
        public double GBP { get; set; }
        public double AMD { get; set; }
        public double BYN { get; set; }
        public double BGN { get; set; }
        public double BRL { get; set; }
        public double HUF { get; set; }
        public double VND { get; set; }
        public double HKD { get; set; }
        public double GEL { get; set; }
        public double DKK { get; set; }
        public double AED { get; set; }
        public double USD { get; set; }
        public double EUR { get; set; }
        public double EGP { get; set; }
        public double INR { get; set; }
        public double IDR { get; set; }
        public double KZT { get; set; }
        public double CAD { get; set; }
        public double QAR { get; set; }
        public double KGS { get; set; }
        public double CNY { get; set; }
        public double MDL { get; set; }
        public double NZD { get; set; }
        public double NOK { get; set; }
        public double PLN { get; set; }
        public double RON { get; set; }
        public double XDR { get; set; }
        public double SGD { get; set; }
        public double TJS { get; set; }
        public double THB { get; set; }
        public double TRY { get; set; }
        public double TMT { get; set; }
        public double UZS { get; set; }
        public double UAH { get; set; }
        public double CZK { get; set; }
        public double SEK { get; set; }
        public double CHF { get; set; }
        public double RSD { get; set; }
        public double ZAR { get; set; }
        public double KRW { get; set; }
        public double JPY { get; set; }
    }

    public class Root
    {
        public string disclaimer { get; set; }
        public string date { get; set; }
        public int timestamp { get; set; }
        public string @base { get; set; }
        public Rates rates { get; set; }
    }


}
