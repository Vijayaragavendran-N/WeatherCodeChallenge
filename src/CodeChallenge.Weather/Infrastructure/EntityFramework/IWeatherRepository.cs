namespace CodeChallenge.Weather.Infrastructure.EntityFramework
{
    using CodeChallenge.Weather.Domain;
    using System;
    using System.Threading.Tasks;

    public interface IWeatherRepository
    {
        Weather? FindById(Guid id);

        void Add(Weather domain);

        void Remove(Weather domain);

        Task SaveAsync();
    }
}
