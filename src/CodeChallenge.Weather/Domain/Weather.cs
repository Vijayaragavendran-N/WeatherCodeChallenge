namespace CodeChallenge.Weather.Domain
{
    using System;

    public class Weather
    {
        public int? TemperatureInCelsius { get; set; }
        public int? RainVolume { get; set; }
        public int? SnowVolume { get; set; }
        public int? WindSpeed { get; set; }
        public string CityName { get; set; }

        public string Description { get; set; }
        public Guid Id { get; private set; }

        public Weather()
        {
            Id = Guid.NewGuid();
        }
    }
}
