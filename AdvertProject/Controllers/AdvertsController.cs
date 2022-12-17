using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvertProject.Models;
using Microsoft.AspNet.Identity;
using ProjectAdvert.Models;

namespace AdvertProject.Controllers
{
    public class AdvertsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Adverts
        public ActionResult Index()
        {
            var adverts = db.Adverts.Include(a => a.User);
            return View(adverts.ToList());
        }

        // GET: Adverts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            return View(advert);
        }

        // GET: Adverts/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.Categories = new SelectList(db.Categories, "ID", "Name");
          
            return View();
        }

        // POST: Adverts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,UserID,Date")] Advert advert)
        {
            if (ModelState.IsValid)
            {
                advert.UserID = User.Identity.GetUserId();
                advert.User = db.Users.Find(advert.UserID);
                advert.Date = DateTime.Now;
                db.Adverts.Add(advert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new SelectList(db.Categories, "ID", "Name");
            return View(advert);
        }

        // GET: Adverts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserId() != advert.UserID)
            {
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.Categories, "ID", "Name");
            return View(advert);
        }

        // POST: Adverts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,UserID,Date")] Advert advert)
        {
            if (User.Identity.GetUserId() != advert.UserID)
            {
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid)
            {
                advert.Date = DateTime.Now;
                db.Entry(advert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Categories = new SelectList(db.Categories, "ID", "Name");
            return View(advert);
        }

        // GET: Adverts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Advert advert = db.Adverts.Find(id);
            if (advert == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserId() != advert.UserID)
            {
                return RedirectToAction("Index");
            }
            return View(advert);
        }
        [Authorize]
        // POST: Adverts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Advert advert = db.Adverts.Find(id);
            if (User.Identity.GetUserId() != advert.UserID)
            {
                return RedirectToAction("Index");
            }
            db.Adverts.Remove(advert);
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
