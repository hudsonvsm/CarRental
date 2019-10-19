using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Car.Rental.Web.App.Extensions;
using Car.Rental.Web.App.Models;
using Car.Rental.Web.App.Models.DataAccessLayer;

namespace Car.Rental.Web.App.Controllers
{
    public class VehicleModelsController : Controller
    {
        private CarRentalDbContext db = new CarRentalDbContext();

        // GET: VehicleModels
        public ActionResult Index(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchString = "";
            }

            var vehicleModels = db.VehicleModels.Include(v => v.VehicleBrand)
                .Where(vm => vm.Name.Contains(searchString)
                || vm.VehicleBrand.Name.Contains(searchString));

            return View(vehicleModels.ToList());
        }

        // GET: VehicleModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }

        // GET: VehicleModels/Create
        public ActionResult Create()
        {
            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Name");
            return View();
        }

        // POST: VehicleModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,VehicleBrandId,Name,MaxPassengers,BigLuggage")] VehicleModel vehicleModel)
        {
            var error = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleModel.Id = Guid.NewGuid();
                    db.VehicleModels.Add(vehicleModel);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    error = ex.GetDeepestMessage();
                }
            }

            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Name", vehicleModel.VehicleBrandId);
            ViewBag.Error = error;

            return View(vehicleModel);
        }

        // GET: VehicleModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Name", vehicleModel.VehicleBrandId);
            return View(vehicleModel);
        }

        // POST: VehicleModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,VehicleBrandId,Name,MaxPassengers,BigLuggage")] VehicleModel vehicleModel)
        {
            var error = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(vehicleModel).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    error = ex.GetDeepestMessage();
                }
            }

            ViewBag.VehicleBrandId = new SelectList(db.VehicleBrands, "Id", "Name", vehicleModel.VehicleBrandId);
            ViewBag.Error = error;

            return View(vehicleModel);
        }

        // GET: VehicleModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            if (vehicleModel == null)
            {
                return HttpNotFound();
            }
            return View(vehicleModel);
        }

        // POST: VehicleModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VehicleModel vehicleModel = db.VehicleModels.Find(id);
            db.VehicleModels.Remove(vehicleModel);
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
