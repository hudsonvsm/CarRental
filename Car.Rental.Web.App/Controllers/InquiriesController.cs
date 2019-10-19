using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Car.Rental.Web.App.Models;
using Car.Rental.Web.App.Models.DataAccessLayer;
using Car.Rental.Web.App.ViewModels;
using CsvHelper;
using RentalModel = Car.Rental.Web.App.Models.Rental;
using SystemType = System.Type;

namespace Car.Rental.Web.App.Controllers
{
    public class InquiriesController : Controller
    {
        private CarRentalDbContext db = new CarRentalDbContext();

        // GET: SearchActiveRentalsByVehicleModel
        public ActionResult SearchActiveRentalsByVehicleModel(string vihicleModel)
        {
            if (string.IsNullOrWhiteSpace(vihicleModel))
            {
                vihicleModel = string.Empty;
            }

            var timedateNew = new DateTime();

            var rentals = this.db.Rentals
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .Where(r =>
                    r.Vehicle.VehicleModel.Name.Equals(vihicleModel)
                    && !r.RentedAt.Equals(timedateNew)
                    && r.ReturnedAt == null
                )
                .ToList();

            var models = this.db.VehicleModels
                .Include(r => r.VehicleBrand)
                .Select(v => new SelectListItem
                {
                    Text = v.Name,
                    Value = v.Name
                })
                .ToList();

            var objects = rentals
            .Select(x => new RentalViewModelClient
            {
                FullName = x.Client.FirstName + " " + x.Client.LastName,
                LicensePlate = x.Vehicle.LicensePlate,
                RentedAt = x.RentedAt,
                VehicleBrandModel = x.Vehicle.VehicleModel.VehicleBrand.Name + " - " + x.Vehicle.VehicleModel.Name
            })
            .ToList<object>();

            var csvString = this.GenerateCsvString(objects);

            return View(new Tuple<List<RentalModel>, List<SelectListItem>, string>(rentals, models, csvString));
        }

        // GET: SearchActiveRentalsByClient
        public ActionResult SearchActiveRentalsByClient(string clientName)
        {
            if (string.IsNullOrWhiteSpace(clientName))
            {
                clientName = string.Empty;
            }

            var timedateNew = new DateTime();

            var rentals = this.db.Rentals
                .Include(r => r.Client)
                .Include(r => r.Vehicle)
                .Where(r => (r.Client.FirstName + " " + r.Client.LastName).Equals(clientName)
                    && !r.RentedAt.Equals(timedateNew)
                    && r.ReturnedAt == null
                )
                .ToList();

            var models = this.db.Clients
                .Select(r => new SelectListItem
                {
                    Text = r.FirstName + " " + r.LastName,
                    Value = r.FirstName + " " + r.LastName
                })
                .ToList();

            var objects = rentals
                .Select(x => new RentalViewModelClient
                {
                    FullName = x.Client.FirstName + " " + x.Client.LastName,
                    LicensePlate = x.Vehicle.LicensePlate,
                    RentedAt = x.RentedAt,
                    VehicleBrandModel = x.Vehicle.VehicleModel.VehicleBrand.Name + " - " + x.Vehicle.VehicleModel.Name
                })
                .ToList<object>();

            var csvString = this.GenerateCsvString(objects);

            return View(new Tuple<List<RentalModel>, List<SelectListItem>, string>(rentals, models, csvString));
        }

        // GET: SearchMostProfitableClient
        public ActionResult SearchMostProfitableClient(int? count)
        {
            var maxCount = 10;
            if (count == null)
            {
                count = maxCount;
            }

            var timedateNew = DateTime.Now;

            var rentals = this.db.Rentals
                .Include(r => r.Vehicle)
                .Include(r => r.Client)
                .ToList()
                .GroupBy(r => r.ClientId)
                .OrderByDescending(gr => gr.Sum(r => this.CalculatePrice(r)))
                .Take(count.Value)
                .ToDictionary(x => x.ToList()[0], x => x.Sum(r => this.CalculatePrice(r)));

            var items = GetSelectListItemsByMaxCount(maxCount, count.Value);

            var objects = rentals
                .Select(x => new RentalMostProfitableClent
                {
                    FullName = x.Key.Client.FirstName + " " + x.Key.Client.LastName,
                    Adrress = x.Key.Client.Address,
                    DriverLicenseValidUntil = x.Key.Client.DriverLicense.ValidUntil,
                    EarnedMoney = x.Value
                })
                .ToList<object>();

            var csvString = this.GenerateCsvString(objects);

            return View(new Tuple<Dictionary<RentalModel, decimal>, List<SelectListItem>, string>(rentals, items, csvString));
        }

        // GET: SearchMostProfitableVehicles
        public ActionResult SearchMostProfitableVehicles(int? count)
        {
            var maxCount = 10;
            if (count == null)
            {
                count = maxCount;
            }

            var timedateNew =  DateTime.Now;

            var rentals = this.db.Rentals
                .Include(r => r.Vehicle)
                .ToList()
                .GroupBy(r => r.VehicleId)
                .OrderByDescending(gr => gr.Sum(r => this.CalculatePrice(r)))
                .Take(count.Value)
                .Select(x => {
                    var rental = x.ToList()[0];
                    var calcDays = x.Sum(r => this.CalculateDays(r));
                    return new RentalMostProfitableVehicle
                    {
                        BrandModel = rental.Vehicle.VehicleModel.VehicleBrand.Name + " - " + rental.Vehicle.VehicleModel.Name,
                        LicensePlate = rental.Vehicle.LicensePlate,
                        PricePerDay = rental.Vehicle.PricePerDay,
                        TotalDaysRented = calcDays,
                        EarnedMoney = calcDays * rental.Vehicle.PricePerDay
                    };
                })
                .ToList();

            var items = GetSelectListItemsByMaxCount(maxCount, count.Value);

            var csvString = this.GenerateCsvString(rentals.ToList<object>());

            return View(new Tuple<List<RentalMostProfitableVehicle>, List<SelectListItem>, string>(rentals, items, csvString));
        }

        // GET: SearchMostRentedVehicles
        public ActionResult SearchMostRentedVehicles(int? count)
        {
            var maxCount = 10;
            if (count == null)
            {
                count = maxCount;
            }

            var rentals = this.db.Rentals
                .Include(r => r.Vehicle)
                .ToList()
                .GroupBy(r => r.VehicleId)
                .OrderByDescending(gr => gr.Count())
                .Take(count.Value)
                .Select(x => new RentalMostRentedVehicle
                {
                    BrandAndModel = x.ToList()[0].Vehicle.VehicleModel.VehicleBrand.Name + " - " + x.ToList()[0].Vehicle.VehicleModel.Name,
                    LicensePlate = x.ToList()[0].Vehicle.LicensePlate,
                    PricePerDay = x.ToList()[0].Vehicle.PricePerDay,
                    TotalDaysRented = x.Sum(r => this.CalculateDays(r)),
                    RentsCount = x.Count()
                })
                .ToList();

            var items = GetSelectListItemsByMaxCount(maxCount, count.Value);
            
            var csvString = this.GenerateCsvString(rentals.ToList<object>());

            return View(new Tuple<List<RentalMostRentedVehicle>, List<SelectListItem>, string>(rentals, items, csvString));
        }

        private decimal CalculatePrice(RentalModel rental)
        {
            return (decimal) this.CalculateDays(rental) * rental.Vehicle.PricePerDay;
        }

        private decimal CalculateDays(RentalModel rental)
        {
            var latestDate = rental.ReturnedAt == null ? DateTime.Now : rental.ReturnedAt.Value;

            return (decimal)Math.Ceiling((latestDate.Date - rental.RentedAt.Date).TotalDays);
        }

        private List<SelectListItem> GetSelectListItemsByMaxCount(int maxCount, int selected)
        {
            var items = new List<SelectListItem>();

            for (int i = 1; i <= maxCount; i++)
            {
                items.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = selected == i
                });
            }

            return items;
        }

        private string GenerateCsvString(List<object> viewModels)
        {
            if (viewModels.Count == 0)
            {
                return string.Empty;
            }

            using (var memoryStream = new MemoryStream())
            {
                using (var streamWriter = new StreamWriter(memoryStream))
                using (var csvWriter = new CsvWriter(streamWriter))
                {
                    csvWriter.WriteRecords(viewModels);
                } // StreamWriter gets flushed here.

                return "data:text/csv;charset=utf-8, " + Encoding.Default.GetString(memoryStream.ToArray());
            }
        }
    }
}
