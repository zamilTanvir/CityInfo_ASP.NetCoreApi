using CityInfo.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasData
                (
                new City("New York City")
                {
                    Id = 1,
                    Description = "The One with the big park"
                },
                new City("Antwerp")
                {
                    Id=2,
                    Description = "The One with the Mosque."
                },
                new City("Paris")
                {
                    Id = 3,
                    Description = "The One with that big tower."
                }
                );
            modelBuilder.Entity<PointOfInterest>().HasData(
                new PointOfInterest("Central Park")
                {
                    Id = 1,
                    CityId = 1,
                    Description = "most visited park"
                },
                new PointOfInterest("Empire State Building")
                {
                    Id = 2,
                    CityId = 1,
                    Description = "A 102-Story skyskrapper"
                },
                 new PointOfInterest("Lake")
                 {
                     Id = 3,
                     CityId = 2,
                     Description = "Trees"
                 },
                  new PointOfInterest("Street")
                  {
                      Id = 4,
                      CityId = 2,
                      Description = "Big."
                  },
                   new PointOfInterest("Restaurant")
                   {
                       Id = 5,
                       CityId = 3,
                       Description = "yummy foods."
                   },
                    new PointOfInterest("ShoppingMall")
                    {
                        Id = 6,
                        CityId = 3,
                        Description = "Skykrapper."
                    }
                );
            base.OnModelCreating(modelBuilder);
        }
    }
}
