using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Car.Rental.Web.App.Models;
using Car.Rental.Web.App.Models.DataAccessLayer;
using RentalModel = Car.Rental.Web.App.Models.Rental;

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

            return View(new Tuple<List<RentalModel>, List<SelectListItem>>(rentals, models));
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

            return View(new Tuple<List<RentalModel>, List<SelectListItem>>(rentals, models));
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
                //.Where(r => r.ReturnedAt != null)
                .ToList();

            var out1 = rentals
                .GroupBy(r => r.ClientId)
                //.OrderByDescending(gr => gr.Sum(r => this.CalculatePrice(r)))
                .Take(count.Value)
                .ToDictionary(x => x.ToList()[0], x => x.Sum(r => this.CalculatePrice(r)))
                .OrderByDescending(gr => gr.Value)
                .ToDictionary(k => k.Key, v => v.Value);

            var items = GetItemsByMaxCount(maxCount, count.Value);

            return View(new Tuple<Dictionary<RentalModel, decimal>, List<SelectListItem>>(out1, items));
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
                //.OrderByDescending(gr => gr.Sum(r => this.CalculatePrice(r)))
                .Take(count.Value)
                .ToDictionary(x => x.ToList()[0], x => x.Sum(r => this.CalculatePrice(r)))
                .OrderByDescending(gr => gr.Value)
                .ToDictionary(k => k.Key, v => v.Value);

            var items = GetItemsByMaxCount(maxCount, count.Value);

            return View(new Tuple<Dictionary<RentalModel, decimal>, List<SelectListItem>>(rentals, items));
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
                .GroupBy(r => r.Vehicle.LicensePlate)
                .OrderByDescending(gr => gr.Count())
                .Take(count.Value)
                .ToDictionary(x => x.ToList()[0], x => x.Count());

            var items = GetItemsByMaxCount(maxCount, count.Value);

            return View(new Tuple<Dictionary<RentalModel, int>, List<SelectListItem>>(rentals, items));
        }

        private decimal CalculatePrice(RentalModel rental)
         {
            var latestDate = rental.ReturnedAt == null ? DateTime.Now : rental.ReturnedAt.Value;

            return (decimal) Math.Ceiling((latestDate.Date - rental.RentedAt.Date).TotalDays) * rental.Vehicle.PricePerDay;
        }

    private List<SelectListItem> GetItemsByMaxCount(int maxCount, int selected)
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
    }
}
