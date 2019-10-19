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
    public class ClientsController : Controller
    {
        private CarRentalDbContext db = new CarRentalDbContext();

        // GET: Clients
        public ActionResult Index(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchString))
            {
                searchString = "";
            }

            var clients = db.Clients.Include(c => c.DriverLicense)
                .Where(r => r.FirstName.Contains(searchString)
                    || r.LastName.Contains(searchString)
                    || r.Address.Contains(searchString)
                    || r.IdentificationNumber.Contains(searchString)
                    || r.DriverLicense.IdentificationNumber.Contains(searchString)
                    || r.DriverLicense.Issuer.Contains(searchString)); 

            return View(clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            ViewBag.DriverLicenseId = new SelectList(db.DriverLicenses, "Id", "IdentificationNumber");
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,IdentificationNumber,Address,DriverLicenseId")] Client client)
        {
            var error = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    client.Id = Guid.NewGuid();
                    db.Clients.Add(client);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    error = ex.GetDeepestMessage();
                }
            }

            ViewBag.DriverLicenseId = new SelectList(db.DriverLicenses, "Id", "IdentificationNumber", client.DriverLicenseId);
            ViewBag.Error = error;

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            ViewBag.DriverLicenseId = new SelectList(db.DriverLicenses, "Id", "IdentificationNumber", client.DriverLicenseId);
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,IdentificationNumber,Address,DriverLicenseId")] Client client)
        {
            var error = string.Empty;

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(client).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException ex)
                {
                    error = ex.GetDeepestMessage();
                }
            }

            ViewBag.DriverLicenseId = new SelectList(db.DriverLicenses, "Id", "IdentificationNumber", client.DriverLicenseId);
            ViewBag.Error = error;

            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
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
