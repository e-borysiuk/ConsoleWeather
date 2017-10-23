using System.Net;
using System.Xml.Linq;

namespace ConsoleWeather
{
    public class APICall
    {
        private string _url;
        private const string ApiKey = "???";
        private XDocument _response;

        public APICall(string cityName)
        {
            CreateUrl(cityName);
            CallApi();
        }

        public XDocument GetResponse()
        {
            return _response;
        }

        private void CreateUrl(string cityName)
        {
            _url = "http://api.openweathermap.org/data/2.5/weather?q="
                   + cityName
                   + "&mode=xml&units=metric&APPID="
                   + ApiKey;
        }

        private void CallApi()
        {
            string xml;
            using (var webClient = new WebClient())
            {
                xml = webClient.DownloadString(_url);
            }
            _response = XDocument.Parse(xml);
        }
    }
}