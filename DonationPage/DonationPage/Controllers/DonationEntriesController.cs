using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using DonationPage.Models;

namespace DonationPage.Controllers
{
    public class DonationEntriesController : Controller
    {
        // private DonationPageContext db = new DonationPageContext();
        private TableManager<DonationEntry> tableManager = new TableManager<DonationEntry>("donationconfirmation");
        private const string PartitionKey = "donation-confirmation";

        // GET: DonationEntries
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Donation stories to approve";

            var allItems = await tableManager.GetAllEntitiesAsync();
            return View(allItems.Where(x => !x.Approved).ToList());
        }

        // GET: DonationEntries/Details/5
        public ActionResult Details(string id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DonationEntry donationEntry = tableManager.GetEntity(PartitionKey, id);

            if (donationEntry == null) {
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
        public async Task<ActionResult> Create([Bind(Include = "DonationID,Honoree,Comments")] DonationEntry donationEntry)
        {
            if (ModelState.IsValid) {
                donationEntry.PartitionKey = PartitionKey;
                donationEntry.RowKey = donationEntry.DonationID;
                donationEntry.Approved = true;

                await tableManager.CreateEntityAsync(donationEntry);

                return RedirectToAction("Index");
            }

            return View(donationEntry);
        }

        public async Task<ActionResult> Stories()
        {
            var allItems = await tableManager.GetAllEntitiesAsync();
            return View(allItems.Where(x => x.Approved).ToList());
        }

        public async Task<ActionResult> FullList()
        {
            ViewBag.Title = "All stories";

            var allItems = await tableManager.GetAllEntitiesAsync();
            return View("Index", allItems);
        }

        // GET: DonationEntries/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationEntry donationEntry = tableManager.GetEntity(PartitionKey, id);
            if (donationEntry == null) {
                return HttpNotFound();
            }
            return View(donationEntry);
        }

        // POST: DonationEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DonationEntry donationEntry)
        {
            if (ModelState.IsValid) {
                tableManager.UpsertEntity(donationEntry);
                return RedirectToAction("Index");
            }
            return View(donationEntry);
        }

        // GET: DonationEntries/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonationEntry donationEntry = tableManager.GetEntity(PartitionKey, id);
            if (donationEntry == null) {
                return HttpNotFound();
            }
            return View(donationEntry);
       }

        // POST: DonationEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DonationEntry donationEntry = tableManager.GetEntity(PartitionKey, id);
            tableManager.DeleteEntity(donationEntry);

            return RedirectToAction("Index");
        }
    }
}
