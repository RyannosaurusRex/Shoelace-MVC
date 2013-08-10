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
    public class PersonController : ShoelaceController
    {
        //
        // GET: /Person/
        public ActionResult Index()
        {
            var tenantId = RouteData.GetTenantId();
            return View(db.PersonRepository.Get(x => x.AccountId == tenantId).ToList());
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.MenuActive = MenuItems.People;
            base.OnActionExecuting(filterContext);
        }

        //
        // GET: /Person/Details/5
        public ActionResult Details(Int32 id)
        {
            var tid = RouteData.GetTenantId();
            Person person = db.PersonRepository.Get().FirstOrDefault(x => x.AccountId == tid && x.Id == id);
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
                person.AccountId = RouteData.GetTenantId();
                db.PersonRepository.Insert(person);
                db.Save();
                return RedirectToAction("Index");
            }

            return View(person);
        }

        //
        // GET: /Person/Edit/5
        public ActionResult Edit(Int32? id)
        {
            var tid = RouteData.GetTenantId();
            Person person = null;
            if (id == null)
            {
                person = new Person();
                person.AccountId = tid;
            }
            else
            {
                person = db.PersonRepository.Get().FirstOrDefault(x => x.AccountId == tid && x.Id == id);
                if (person == null)
                {
                    return HttpNotFound();
                }
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
                // Sync phone records.

                if (person.Id == 0)
                {
                    db.PersonRepository.Insert(person);
                }
                else
                {
                    db.PersonRepository.Update(person);
                }
                db.Save();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        //
        // GET: /Person/Delete/5
        public ActionResult Delete(Int32 id)
        {
            Person person = db.PersonRepository.GetByID(id);
            if (person == null && person.AccountId == RouteData.GetTenantId())
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
            var tid = RouteData.GetTenantId();
            Person person = db.PersonRepository.Get().FirstOrDefault(x => x.Id == id && x.AccountId == tid);
            db.PersonRepository.Delete(person);
            db.Save();
            return RedirectToAction("Index");
        }
    }
}
