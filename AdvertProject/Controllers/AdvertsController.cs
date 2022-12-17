using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvertProject.Migrations;
using AdvertProject.Models;
using Microsoft.AspNet.Identity;
using ProjectAdvert.Models;
using WebGrease.Css.Extensions;

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
            ViewBag.Categories = new MultiSelectList(db.Categories, "ID", "Name");
  
            return View();
        }

        // POST: Adverts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content,UserID,Date")] Advert advert, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                String categoryIds = form["categories"];
                if (categoryIds != null)
                {
                    List<string> choosesList = categoryIds.Split(',').ToList();
                    foreach(string category in choosesList)
                    {
                        int categoryId = Int32.Parse(category);
                        AdvertCategory ac= new AdvertCategory();
                        ac.AdvertID = advert.ID;
                        ac.CategoryID = categoryId;
                        db.AdvertCategories.Add(ac);
                    }
                }
                advert.UserID = User.Identity.GetUserId();
                advert.User = db.Users.Find(advert.UserID);
                advert.Date = DateTime.Now;
                db.Adverts.Add(advert);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = new MultiSelectList(db.Categories, "ID", "Name");
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
            Advert advert = db.Adverts.Where(x => x.ID == id).Include(x => x.categories).FirstOrDefault();
            if (advert == null)
            {
                return HttpNotFound();
            }
            if (User.Identity.GetUserId() != advert.UserID)
            {
                return RedirectToAction("Index");
            }

            List<string> selectedCategories = new List<string>();
            foreach (AdvertCategory category in advert.categories.ToList())
            {
                selectedCategories.Add(category.CategoryID.ToString());
            }


            ViewBag.Categories = new MultiSelectList(db.Categories, "ID", "Name", selectedCategories);
            return View(advert);
        }

        // POST: Adverts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content,UserID,Date")] Advert advert, FormCollection form)
        {
            if (User.Identity.GetUserId() != advert.UserID)
            {
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                foreach(AdvertCategory ac in db.AdvertCategories.Where(x => x.AdvertID == advert.ID))
                {
                    db.AdvertCategories.Remove(ac);
                }
               
                String categoryIds = form["categories"];
                if (categoryIds != null)
                {
                    List<string> choosesList = categoryIds.Split(',').ToList();
                    foreach (string category in choosesList)
                    {
                        int categoryId = Int32.Parse(category);
                        AdvertCategory ac = new AdvertCategory();
                        ac.AdvertID = advert.ID;
                        ac.CategoryID = categoryId;
                        db.AdvertCategories.Add(ac);
                    }
                }
                advert.Date = DateTime.Now;
                db.Entry(advert).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            List<string> selectedCategories = new List<string>();
            foreach (AdvertCategory category in advert.categories.ToList())
            {
                selectedCategories.Add(category.CategoryID.ToString());
            }

            ViewBag.Categories = new MultiSelectList(db.Categories, "ID", "Name", selectedCategories);
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
