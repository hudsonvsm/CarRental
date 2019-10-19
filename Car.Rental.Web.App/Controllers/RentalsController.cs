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
    public class RentalsController : Controller
    {
        private CarRentalDbContext db = new CarRentalDbContext();

        // GET: Rentals
        public ActionResult Index(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchString = "";
            }

            var rentals = db.Rentals.Include(r => r.Client).Include(r => r.Vehicle)
                .Where(r => r.Client.FirstName.Contains(searchString) 
                    || r.Client.LastName.Contains(searchString) 
                    || r.Vehicle.VehicleModel.Name.Contains(searchString) 
                    || r.Vehicle.VehicleModel.VehicleBrand.Name.Contains(searchString)
                    || r.Vehicle.LicensePlate.Contains(searchString));

            return View(rentals.ToList());
        }

        // GET: Rentals/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalModel rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            return View(rental);
        }

        // GET: Rentals/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName");
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "LicensePlate");

            return View(new RentalModel());
        }

        // POST: Rentals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,RentedAt,ReturnedAt,VehicleId,ClientId")] RentalModel rental)
        {
            if (ModelState.IsValid)
            {
                rental.Id = Guid.NewGuid();
                db.Rentals.Add(rental);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName", rental.ClientId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "LicensePlate", rental.VehicleId);
            return View(rental);
        }

        // GET: Rentals/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalModel rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName", rental.ClientId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "LicensePlate", rental.VehicleId);
            return View(rental);
        }

        // POST: Rentals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,RentedAt,ReturnedAt,VehicleId,ClientId")] RentalModel rental)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rental).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "Id", "FirstName", rental.ClientId);
            ViewBag.VehicleId = new SelectList(db.Vehicles, "Id", "LicensePlate", rental.VehicleId);
            return View(rental);
        }

        // GET: Rentals/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalModel rental = db.Rentals.Find(id);
            if (rental == null)
            {
                return HttpNotFound();
            }
            return View(rental);
        }

        // POST: Rentals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            RentalModel rental = db.Rentals.Find(id);
            db.Rentals.Remove(rental);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
