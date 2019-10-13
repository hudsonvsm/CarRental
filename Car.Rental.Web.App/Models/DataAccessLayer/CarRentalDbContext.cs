using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Car.Rental.Web.App.Models.DataAccessLayer
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext() : base("Name=CarRentalDBConnectionString")
        {
            Database.SetInitializer(new CarRentalInitializer());
        }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<VehicleBrand> VehicleBrands { get; set; }

        public DbSet<VehicleModel> VehicleModels { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<DriverLicense> DriverLicenses { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}