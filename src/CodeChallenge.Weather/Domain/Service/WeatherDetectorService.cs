namespace CodeChallenge.Weather.Domain.Service
{
    using CodeChallenge.Weather.Infrastructure.EntityFramework;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public class WeatherDetectorService : IWeatherRepository
    {
        WeatherContext weatherContext = new WeatherContext();
        Weather weather = new Weather();
        ILogger<WeatherDetectorService> logger;
        public void Add(Weather domain)
        {
            try
            {
                weather.CityName = domain.CityName;
                weather.RainVolume = domain.RainVolume;
                weather.SnowVolume = domain.SnowVolume;
                weather.TemperatureInCelsius = domain.TemperatureInCelsius;
                weather.WindSpeed = domain.WindSpeed;
                weather.Description = domain.Description;
                weatherContext.Weather.Add(weather);
                weatherContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);                
            }
        }

        public Weather? FindById(Guid id)
        {
            try
            {
                var description = weatherContext.Weather.Where(x => x.Id == id).Select(x => x.Description).ToList();
                weather.Description = description[0];                
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
            }
            return weather;
        }

        public string GetWeather(SensorsWeather sensor)
        {
            string result = string.Empty;
            try
            {
                if (sensor.TemperatureInCelsius == 20)
                {
                    result = "is having a good weather";                  
                }
            }
            catch (Exception ex)
            {
                logger.LogInformation(ex.Message);
            }
            return result;
        }

        public void Remove(Weather domain)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync()
        {
            throw new NotImplementedException();
        }
    }


}
