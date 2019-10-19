using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Car.Rental.Web.App.Models.DataAccessLayer
{
    public class CarRentalInitializer : CreateDatabaseIfNotExists<CarRentalDbContext>
    {
        protected override void Seed(CarRentalDbContext context)
        {

            var driverLicenses = this.GenerateDriversLicenses();

            context.DriverLicenses.AddRange(driverLicenses);

            var clients = this.GenerateClients(driverLicenses);

            context.Clients.AddRange(clients);

            var vbrands = this.GenerateBrands();

            context.VehicleBrands.AddRange(vbrands);

            var vmodels = this.GenerateModels(vbrands);

            context.VehicleModels.AddRange(vmodels);

            var vehacles = this.GenerateVehicles(vmodels);

            context.Vehicles.AddRange(vehacles);

            var rentals = this.GenerateRents(vehacles, clients);

            context.Rentals.AddRange(rentals);

            context.SaveChanges();
        }

        private List<DriverLicense> GenerateDriversLicenses()
        {
            return new List<DriverLicense>(){
                new DriverLicense()
                {
                    Id = Guid.NewGuid(),
                    IdentificationNumber = "1234576543",
                    Issuer = "MVR - Sofia",
                    ValidUntil =  DateTime.Now
                },
                new DriverLicense()
                {
                    Id = Guid.NewGuid(),
                    IdentificationNumber = "84785879879",
                    Issuer = "MVR - Varna",
                    ValidUntil =  DateTime.Now
                },
                new DriverLicense()
                {
                    Id = Guid.NewGuid(),
                    IdentificationNumber = "5919835646",
                    Issuer = "MVR - Burgas",
                    ValidUntil =  DateTime.Now
                },
                new DriverLicense()
                {
                    Id = Guid.NewGuid(),
                    IdentificationNumber = "6662245565",
                    Issuer = "MVR - Vidin",
                    ValidUntil =  DateTime.Now
                },
                new DriverLicense()
                {
                    Id = Guid.NewGuid(),
                    IdentificationNumber = "7946529652",
                    Issuer = "MVR - Plovdiv",
                    ValidUntil =  DateTime.Now
                },
            };
        }

        private List<Client> GenerateClients(List<DriverLicense> driverLicenses)
        {
            return new List<Client>
            {
                new Client{
                    Id = Guid.NewGuid(),
                    FirstName = "Nikolaj",
                    LastName = "Dimitrov",
                    Address = "Sofiq v centyra",
                    DriverLicense = driverLicenses[0],
                    IdentificationNumber = "4676572345452",
                },
                new Client{
                    Id = Guid.NewGuid(),
                    FirstName = "Pesho",
                    LastName = "Peshev",
                    Address = "Varna Mladost",
                    DriverLicense = driverLicenses[1],
                    IdentificationNumber = "1597536995534",
                },
                new Client{
                    Id = Guid.NewGuid(),
                    FirstName = "Gojko",
                    LastName = "Minchev",
                    Address = "Burgas kraj moreto",
                    DriverLicense = driverLicenses[2],
                    IdentificationNumber = "1235896455853",
                },
                new Client{
                    Id = Guid.NewGuid(),
                    FirstName = "Ivan",
                    LastName = "Penchev",
                    Address = "Vidin za Baba Vida",
                    DriverLicense = driverLicenses[3],
                    IdentificationNumber = "785694125563",
                },
                new Client{
                    Id = Guid.NewGuid(),
                    FirstName = "Vladimir",
                    LastName = "Razni",
                    Address = "Plovdiv pod tepetata",
                    DriverLicense = driverLicenses[4],
                    IdentificationNumber = "1596854472269",
                },
            };
        }

        private List<VehicleBrand> GenerateBrands()
        {
            return new List<VehicleBrand>()
            {
                new VehicleBrand()
                {
                    Id = Guid.NewGuid(),
                    Name = "BMW",
                },
                new VehicleBrand()
                {
                    Id = Guid.NewGuid(),
                    Name = "Toyota",
                },
                new VehicleBrand()
                {
                    Id = Guid.NewGuid(),
                    Name = "VW",
                },
                new VehicleBrand()
                {
                    Id = Guid.NewGuid(),
                    Name = "Renault",
                }
            };
        }

        private List<VehicleModel> GenerateModels(List<VehicleBrand> vbrands)
        {
            return new List<VehicleModel>() {
                new VehicleModel
                {
                    Id = Guid.NewGuid(),
                    MaxPassengers = 5,
                    VehicleBrand = vbrands[0],
                    VehicleBrandId = vbrands[0].Id,
                    Name = "i350 M",
                    BigLuggage = false
                },
                new VehicleModel
                {
                    Id = Guid.NewGuid(),
                    MaxPassengers = 4,
                    VehicleBrand = vbrands[1],
                    VehicleBrandId = vbrands[1].Id,
                    Name = "Aygo",
                    BigLuggage = false
                },
                new VehicleModel
                {
                    Id = Guid.NewGuid(),
                    MaxPassengers = 5,
                    VehicleBrand = vbrands[2],
                    VehicleBrandId = vbrands[2].Id,
                    Name = "Golf",
                    BigLuggage = false
                },
                new VehicleModel
                {
                    Id = Guid.NewGuid(),
                    MaxPassengers = 7,
                    VehicleBrand = vbrands[3],
                    VehicleBrandId = vbrands[3].Id,
                    Name = "Scenic",
                    BigLuggage = true
                },
                new VehicleModel
                {
                    Id = Guid.NewGuid(),
                    MaxPassengers = 7,
                    VehicleBrand = vbrands[0],
                    VehicleBrandId = vbrands[0].Id,
                    Name = "X6",
                    BigLuggage = true
                }
            };
        }

        private List<Vehicle> GenerateVehicles(List<VehicleModel> vmodels)
        {
            return new List<Vehicle>()
            {
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "CB 1122 CA",
                    VehicleModel = vmodels[0],
                    VehicleModelId = vmodels[0].Id,
                    Type = Type.Car,
                    PricePerDay = 400,
                    TechnicalInspectionDoneAt = DateTime.Now,
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "CB 5698 CA",
                    VehicleModel = vmodels[1],
                    VehicleModelId = vmodels[1].Id,
                    Type = Type.Car,
                    PricePerDay = 100,
                    TechnicalInspectionDoneAt = DateTime.Now,
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "CB 5236 CA",
                    VehicleModel = vmodels[2],
                    VehicleModelId = vmodels[2].Id,
                    Type = Type.Car,
                    PricePerDay = 200,
                    TechnicalInspectionDoneAt = DateTime.Now,
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "CB 5769 CA",
                    VehicleModel = vmodels[3],
                    VehicleModelId = vmodels[3].Id,
                    Type = Type.MiniBus,
                    PricePerDay = 300,
                    TechnicalInspectionDoneAt = DateTime.Now,
                },
                new Vehicle
                {
                    Id = Guid.NewGuid(),
                    LicensePlate = "CB 4521 CA",
                    VehicleModel = vmodels[4],
                    VehicleModelId = vmodels[4].Id,
                    Type = Type.MiniBus,
                    PricePerDay = 500,
                    TechnicalInspectionDoneAt = DateTime.Now,
                },
            };
        }

        private List<Rental> GenerateRents(List<Vehicle> vehicles, List<Client>clients)
        {
            return new List<Rental>()
            {
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[0],
                    ClientId = clients[0].Id,
                    RentedAt = new DateTime(2018, 5, 10),
                    ReturnedAt = new DateTime(2018, 6, 10),
                    Vehicle = vehicles[0],
                    VehicleId = vehicles[0].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[0],
                    ClientId = clients[0].Id,
                    RentedAt = new DateTime(2018, 4, 11),
                    Vehicle = vehicles[4],
                    VehicleId = vehicles[4].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[1],
                    ClientId = clients[1].Id,
                    RentedAt = new DateTime(2017, 1, 10),
                    ReturnedAt = new DateTime(2017, 4, 22),
                    Vehicle = vehicles[1],
                    VehicleId = vehicles[1].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[1],
                    ClientId = clients[1].Id,
                    RentedAt = new DateTime(2018, 5, 10),
                    Vehicle = vehicles[3],
                    VehicleId = vehicles[3].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[2],
                    ClientId = clients[2].Id,
                    RentedAt = new DateTime(2017, 7, 10),
                    ReturnedAt = new DateTime(2018, 6, 22),
                    Vehicle = vehicles[3],
                    VehicleId = vehicles[3].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[3],
                    ClientId = clients[3].Id,
                    RentedAt = new DateTime(2016, 1, 11),
                    ReturnedAt = new DateTime(2018, 4, 11),
                    Vehicle = vehicles[3],
                    VehicleId = vehicles[3].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[3],
                    ClientId = clients[3].Id,
                    RentedAt = new DateTime(2011, 5, 10),
                    ReturnedAt = new DateTime(2012, 2, 22),
                    Vehicle = vehicles[2],
                    VehicleId = vehicles[2].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[4],
                    ClientId = clients[4].Id,
                    RentedAt = new DateTime(2013, 9, 11),
                    ReturnedAt = new DateTime(2015, 3, 10),
                    Vehicle = vehicles[4],
                    VehicleId = vehicles[4].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[4],
                    ClientId = clients[4].Id,
                    RentedAt = new DateTime(2016, 2, 10),
                    ReturnedAt = new DateTime(2017, 7, 22),
                    Vehicle = vehicles[0],
                    VehicleId = vehicles[0].Id
                },
                new Rental
                {
                    Id = Guid.NewGuid(),
                    Client = clients[4],
                    ClientId = clients[4].Id,
                    RentedAt = new DateTime(2013, 5, 10),
                    ReturnedAt = new DateTime(2014, 6, 10),
                    Vehicle = vehicles[2],
                    VehicleId = vehicles[2].Id
                },
            };
        }

    }
}