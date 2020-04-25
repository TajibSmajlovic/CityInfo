using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Contexts
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData(new City()
            {
                Id = 1,
                Name = "Visoko",
                Description = "Tamo gdje su piramide."
            });

            modelBuilder.Entity<PointOfInterest>().HasData(new PointOfInterest()
            {
                Id = 1,
                CityId = 1,
                Name = "Piramida Sunca",
                Description = "Jes piramida svega mi!!!"
            }
                );
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointOfInterests { get; set; }
    }
}