using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShoelaceMVC.Models;

namespace ShoelaceMVC.Controllers
{
    public class PersonController : Controller
    {
        private ShoelaceDbContext db = new ShoelaceDbContext();

        //
        // GET: /Person/
        public ActionResult Index()
        {
            var tenantId = RouteData.GetTenantId();
            return View(db.People.Where(x => x.Account.Id == tenantId).ToList());
        }

        //
        // GET: /Person/Details/5
        public ActionResult Details(Int32 id)
        {
            Person person = db.People.FirstOrDefault(x => x.Account.Id == RouteData.GetTenantId() && x.Id == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        //
        // GET: /Person/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Person person)
        {
            if (ModelState.IsValid)
            {
                person.Account.Id = RouteData.GetTenantId();
                db.People.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        //
        // GET: /Person/Edit/5
        public ActionResult Edit(Int32 id)
        {
            Person person = db.People.FirstOrDefault(x => x.Account.Id == RouteData.GetTenantId() && x.Id == id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        //
        // POST: /Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Person person)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        //
        // GET: /Person/Delete/5
        public ActionResult Delete(Int32 id)
        {
            Person person = db.People.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        //
        // POST: /Person/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Int32 id)
        {
            Person person = db.People.FirstOrDefault(x => x.Id == id && x.Account.Id == RouteData.GetTenantId());
            db.People.Remove(person);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
