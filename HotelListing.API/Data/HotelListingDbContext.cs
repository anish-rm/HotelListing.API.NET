using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace HotelListing.API.Data
{
    public class HotelListingDbContext : DbContext //Dbcontext class comes from Entity Framework. We are inheriting from it.
    {
        public HotelListingDbContext(DbContextOptions options) : base(options) //these optuons comes from options tht we defined in Program.cs
        {

        }

        // we have defined tables in Hotel and Country but we want our database to know that

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        // we have done everything but we havent created db yet. To do so goto Package manager console
        // add-migration InitialMigration
        // This will create a separate migration folder in that it will have all commands like createTable, etc


        //SEEDING SOME DEFAULT DATA
        //Default data is added when db is created for the first time

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //we have to call the base first
            //it gives access to the base functionality
            base.OnModelCreating(modelBuilder);
            //seed country first becuase hotel is dependent on it
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id=1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Island",
                    ShortName = "CI"
                }
            );

            modelBuilder.Entity<Hotel>().HasData(
                new Hotel
                {
                    Id = 1,
                    Name = "Sandals Resort and Spa",
                    Address = "Negril",
                    CountryId = 1,
                    Rating = 4.5
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Comfort Suites",
                    Address = "George Town",
                    CountryId = 3,
                    Rating = 4.3
                },
                new Hotel
                {
                    Id = 3,
                    Name = "Grand Palldium",
                    Address = "Nassua",
                    CountryId = 2,
                    Rating = 4
                }
            );

            //after this add-migration 
            // By giving meaningful name for each migration it will be easy for us to rollback if there is any errors
        }

    }
}
