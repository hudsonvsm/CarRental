using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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

        // GET: SearchActiveRentalsByVihicleModel
        public ActionResult SearchActiveRentalsByVihicleModel(string vihicleModel)
        {
            if (string.IsNullOrWhiteSpace(vihicleModel))
            {
                vihicleModel = "";
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
        public ActionResult SearchActiveRentalsByClient()
        {
            return View();
        }

        // GET: SearchPastRentalsByVihicleModelAndDate
        public ActionResult SearchPastRentalsByVihicleModelAndDate()
        {
            return View();
        }

        // GET: SearchPastRentalsByClientAndDate
        public ActionResult SearchPastRentalsByClientAndDate()
        {
            return View();
        }

        // GET: SearchMostRentedVihicles
        public ActionResult SearchMostRentedVihicles()
        {
            return View();
        }
    }
}
