namespace CodeChallenge.Weather.Domain
{
    using System;

    public class Weather
    {
        public Guid Id { get; private set; }

        public Weather()
        {
            Id = Guid.NewGuid();
        }
    }
}
