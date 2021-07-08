using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using LotteCinema.C2.Common.Enum;
using LotteCinema.C2.Common.Model;

namespace LotteCinema.C2.Common.Class
{
    public class Weather
    {
        #region Variable

        private readonly string weatherForecastUrl = "http://www.kma.go.kr/wid/queryDFSRSS.jsp?zone={0}";

        #endregion

        #region Constructor

        public Weather(string zone = "1171069000")
        {
            weatherForecastUrl = string.Format(weatherForecastUrl, zone);
        }

        #endregion

        #region Public Method

        public async Task<WeatherData> GetCurrentData()
        {
            if (!NetworkInterface.GetIsNetworkAvailable()) return null;

            List<WeatherData> weatherList = null;

            using (WebClient wc = new WebClient())
            {
                wc.Encoding = ASCIIEncoding.UTF8;

                var result = await wc.DownloadStringTaskAsync(this.weatherForecastUrl);

                if (!string.IsNullOrWhiteSpace(result))
                {
                    XDocument xd = XDocument.Parse(result);
                    XElement root = xd.Root.Elements().FirstOrDefault();

                    foreach (XElement rootElement in root.Elements())
                    {
                        if (rootElement.Name.LocalName.Equals("item") == false)
                        {
                            continue;
                        }

                        foreach (XElement itemElement in rootElement.Elements())
                        {
                            if (itemElement.Name.LocalName.Equals("description") == false)
                            {
                                continue;
                            }

                            foreach (XElement xe in itemElement.Elements())
                            {
                                if (xe.Name.LocalName.Equals("body"))
                                {
                                    weatherList = (from data in xe.Descendants("data")
                                                   select new WeatherData
                                                   {
                                                       Temp = data.Element("temp").Value,
                                                       WeatherKind = ConvertToWeatherKind(data.Element("wfEn").Value)
                                                   }).ToList();
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            if (weatherList != null && weatherList.Count > 0)
            {
                return weatherList[0];
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Private Method

        private WeatherKind ConvertToWeatherKind(string str)
        {
            switch (str)
            {
                case "Clear": return WeatherKind.Clear;
                case "Partly Cloudy": return WeatherKind.PartlyCloudy;
                case "Mostly Cloudy": return WeatherKind.MostlyCloudy;
                case "Cloudy": return WeatherKind.Cloudy;
                case "Rain": return WeatherKind.Rain;
                case "Snow/Rain": return WeatherKind.SnowRain;
                case "Snow": return WeatherKind.Snow;
                default: return WeatherKind.Clear;
            }
        }

        #endregion
    }
}