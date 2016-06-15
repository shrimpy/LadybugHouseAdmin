using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DonationPage.Models;

namespace DonationPage.Controllers
{
    public class DonationEntriesController : Controller
    {
        private DonationPageContext db = new DonationPageContext();

        // GET: DonationEntries
        public ActionResult Index()
        {
            ViewBag.Title = "Donation stories to approve";
            return View(db.DonationEntries.Where(d => !d.Approved).ToList());
        }

        // GET: DonationEntries/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationEntry donationEntry = db.DonationEntries.Find(id);
            if (donationEntry == null)
            {
                return HttpNotFound();
            }
            return View(donationEntry);
        }

        // GET: DonationEntries/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DonationEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DonationID,DonationTitle,Text")] DonationEntry donationEntry)
        {
            if (ModelState.IsValid)
            {
                db.DonationEntries.Add(donationEntry);
                donationEntry.Approved = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(donationEntry);
        }

        public ActionResult Stories()
        {
            return View(db.DonationEntries.Where(d => d.Approved).ToList());
        }

        public ActionResult FullList()
        {
            ViewBag.Title = "All stories";
            return View("Index", db.DonationEntries.ToList());
        }

        // GET: DonationEntries/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationEntry donationEntry = db.DonationEntries.Find(id);
            if (donationEntry == null)
            {
                return HttpNotFound();
            }
            return View(donationEntry);
        }

        // POST: DonationEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DonationID,DonationTitle,Text,Approved")] DonationEntry donationEntry)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donationEntry).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donationEntry);
        }

        // GET: DonationEntries/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationEntry donationEntry = db.DonationEntries.Find(id);
            if (donationEntry == null)
            {
                return HttpNotFound();
            }
            return View(donationEntry);
        }

        // POST: DonationEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DonationEntry donationEntry = db.DonationEntries.Find(id);
            db.DonationEntries.Remove(donationEntry);
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
