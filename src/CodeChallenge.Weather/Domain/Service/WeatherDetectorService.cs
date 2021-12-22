namespace CodeChallenge.Weather.Domain.Service
{
    using System;

    public class WeatherDetectorService
    {
        public string GetWeather(SensorsWeather sensor)
        {
            if (sensor.TemperatureInCelsius == 20)
            {
                return "is having a good weather";
            }

            throw new NotImplementedException();
        }
    }
}
