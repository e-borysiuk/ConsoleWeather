using System.Xml.Linq;

namespace ConsoleWeather
{
    public class CityWeather
    {
        private string _cityName;
        private string _coords;
        private string _country;

        private string _temperature;
        private string _humidity;
        private string _pressure;

        private string _windSpeed;

        public CityWeather()
        {

        }

        public string FormatInformation()
        {
            return "Miejscowość: " + _cityName + ", " + _country + "\n" +
                   "Położenie: " + _coords + "\n" +
                   "Temperatura: " + _temperature + "°C" + "\n" +
                   "Wilogtność: " + _humidity + "%" + "\n" +
                   "Ciśnienie: " + _pressure + " hPa" + "\n" +
                   "Prędkość wiatru: " + _windSpeed + "m/s";
        }

        public void ConvertResponse(XDocument response)
        {
            _cityName = response.Root.Element("city").Attribute("name").Value;
            _coords = response.Root.Element("city").Element("coord").Attribute("lat").Value + "N "
                      + response.Root.Element("city").Element("coord").Attribute("lon").Value + "S";
            _country = response.Root.Element("city").Element("country").Value;
            _temperature = response.Root.Element("temperature").Attribute("value").Value;
            _humidity = response.Root.Element("humidity").Attribute("value").Value;
            _pressure = response.Root.Element("pressure").Attribute("value").Value;
            _windSpeed = response.Root.Element("wind").Element("speed").Attribute("value").Value;
        }
    }
}