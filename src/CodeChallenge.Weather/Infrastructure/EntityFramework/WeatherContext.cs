namespace CodeChallenge.Weather.Infrastructure.EntityFramework
{
    using CodeChallenge.Weather.Domain;
    using Microsoft.EntityFrameworkCore;

    public class WeatherContext : DbContext
    {
        public virtual DbSet<Weather> Weather { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public WeatherContext()
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
        }

#pragma warning disable CS8618 
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Do nothing because of X and Y.            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
