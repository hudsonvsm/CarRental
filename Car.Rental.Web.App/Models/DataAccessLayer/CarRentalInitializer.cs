using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Car.Rental.Web.App.Models.DataAccessLayer
{
    public class CarRentalInitializer : CreateDatabaseIfNotExists<CarRentalDbContext>
    {
        protected override void Seed(CarRentalDbContext context)
        {
            var driverLicense = new DriverLicense()
            {
                Id = Guid.NewGuid(),
                IdentificationNumber = "1234576543",
                Issuer = "MVR - Sofia",
                ValidUntil =  DateTime.Now
            };

            context.DriverLicenses.Add(driverLicense);

            var clients = new List<Client>
            {
                new Client{
                    Id = Guid.NewGuid(),
                    FirstName = "Valentin",
                    LastName = "Mladenov",
                    Address = "Sofiq Lulin",
                    DriverLicense = driverLicense,
                    IdentificationNumber = "4676572345452",
                },
            };

            clients.ForEach(c => context.Clients.Add(c));

            var vbrand = new VehicleBrand{
                Id = Guid.NewGuid(),
                Name = "BMW",
            };

            context.VehicleBrands.Add(vbrand);

            var vmodel = new VehicleModel
            {
                Id = Guid.NewGuid(),
                MaxPassengers = 5,
                VehicleBrand = vbrand,
                VehicleBrandId = vbrand.Id,
                Name = "i350 M",
                BigLuggage = false
            };

            context.VehicleModels.Add(vmodel);

            var vehacle = new Vehicle
            {
                Id = Guid.NewGuid(),
                LicensePlate = "XX 112233 MM",
                VehicleModel = vmodel,
                VehicleModelId = vmodel.Id,
                Type = Type.Car,
                PricePerDay = 300,
                TechnicalInspectionDoneAt = DateTime.Now,

            };

            context.Vehicles.Add(vehacle);

            var rental = new Rental
            {
                Id = Guid.NewGuid(),
                Client = clients[0],
                ClientId = clients[0].Id,
                RentedAt = DateTime.Now,
                ReturnedAt = DateTime.Now,
                Vehicle = vehacle,
                VehicleId = vehacle.Id
            };

            context.Rentals.Add(rental);

            context.SaveChanges();
        }
    }
}