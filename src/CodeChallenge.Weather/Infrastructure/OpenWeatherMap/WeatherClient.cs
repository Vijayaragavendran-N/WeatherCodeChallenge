using CodeChallenge.Weather.Domain.Service;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeChallenge.Weather.Infrastructure.OpenWeatherMap
{
    public class WeatherClient : IWeatherClient
    {
        ILogger<WeatherClient> logger;
        public WeatherClient()
        {

        }
        public async Task<Domain.Weather> GetWeatherAsync(string city)
        {
            Domain.Weather weatherResponse = new Domain.Weather();
            HttpClient client = new HttpClient();
            try
            {                
                HttpResponseMessage res = await client.GetAsync("https://api.openweathermap.org/data/2.5/weather?q=" + city + "&units=metric&APPID=1b9c96d2448f9204d210ddd5ac192dc1");
                var apiResponse = await res.Content.ReadAsStringAsync();
                var json = JObject.Parse(apiResponse);
                if (json != null && json["cod"].ToString() != "404" && json["cod"].ToString() != "400")
                {
                    weatherResponse.TemperatureInCelsius = json["main"]["temp"] != null ? (int)json["main"]["temp"] : null;
                    weatherResponse.RainVolume = json["rain"] != null && json["rain"]["1h"] != null ? (int)json["rain"]["1h"] : null;
                    weatherResponse.SnowVolume = json["snow"] != null && json["snow"]["1h"] != null ? (int)json["snow"]["1h"] : null;
                    weatherResponse.WindSpeed = json["wind"] != null && json["wind"]["speed"] != null ? (int)json["wind"]["speed"] : null;
                    weatherResponse.CityName = city;
                    weatherResponse.Description = (string)json["weather"][0]["main"];
                    if (weatherResponse != null)
                    {
                        WeatherDetectorService weatherDetectorService = new WeatherDetectorService();
                        weatherDetectorService.Add(weatherResponse);
                    }
                }               
            }
            catch(Exception ex)
            {
                logger.LogInformation(ex.Message);
            }
            return weatherResponse;
        }
    }
}
