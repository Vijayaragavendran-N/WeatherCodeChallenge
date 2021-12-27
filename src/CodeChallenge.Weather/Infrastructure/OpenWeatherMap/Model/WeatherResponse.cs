namespace CodeChallenge.Weather.Infrastructure.Model
{
    public class WeatherResponse
    {
        public int? TemperatureInCelsius { get; set; }
        public int? RainVolume { get; set; }
        public int? SnowVolume { get; set; }
        public int? WindSpeed { get; set; }
        public string CityName { get; set; }
    }
}