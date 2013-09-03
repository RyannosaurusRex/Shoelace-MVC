using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoelaceMVC.Controllers
{
    public class PhoneController : ShoelaceController
    {
        //
        // GET: /Phone/
        public ActionResult Index()
        {
            return View();
        }
        public class TwilioResponse
        {
            public string SmsSid { get; set; }
            public string AccountSid { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Body { get; set; }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult IncomingSmsMessage(TwilioResponse message)
        {
            db.IncomingPhoneRepository.Insert(new IncomingPhone()
            {
                Message = message.Body,
                PhoneNumber = message.From
            });
            db.Save();
            return Json("Success");
        }

        //
        // GET: /Phone/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Phone/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Phone/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Phone/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Phone/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Phone/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Phone/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}