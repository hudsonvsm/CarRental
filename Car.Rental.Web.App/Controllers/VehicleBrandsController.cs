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
    public class VehicleBrandsController : Controller
    {
        private CarRentalDbContext db = new CarRentalDbContext();

        // GET: VehicleBrands
        public ActionResult Index(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchString = "";
            }

            var vehicleBrands = db.VehicleBrands
                .Where(vb => vb.Name.Contains(searchString));

            return View(vehicleBrands.ToList());
        }

        // GET: VehicleBrands/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleBrand vehicleBrand = db.VehicleBrands.Find(id);
            if (vehicleBrand == null)
            {
                return HttpNotFound();
            }
            return View(vehicleBrand);
        }

        // GET: VehicleBrands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VehicleBrands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] VehicleBrand vehicleBrand)
        {
            var error = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    vehicleBrand.Id = Guid.NewGuid();
                    db.VehicleBrands.Add(vehicleBrand);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    error = ex.GetDeepestMessage();
                }
            }

            ViewBag.Error = error;

            return View(vehicleBrand);
        }

        // GET: VehicleBrands/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleBrand vehicleBrand = db.VehicleBrands.Find(id);
            if (vehicleBrand == null)
            {
                return HttpNotFound();
            }
            return View(vehicleBrand);
        }

        // POST: VehicleBrands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] VehicleBrand vehicleBrand)
        {
            var error = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(vehicleBrand).State = EntityState.Modified;
                    db.VehicleBrands.Add(vehicleBrand);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    error = ex.GetDeepestMessage();
                }
            }

            ViewBag.Error = error;

            return View(vehicleBrand);
        }

        // GET: VehicleBrands/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VehicleBrand vehicleBrand = db.VehicleBrands.Find(id);
            if (vehicleBrand == null)
            {
                return HttpNotFound();
            }
            return View(vehicleBrand);
        }

        // POST: VehicleBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            VehicleBrand vehicleBrand = db.VehicleBrands.Find(id);
            db.VehicleBrands.Remove(vehicleBrand);
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
