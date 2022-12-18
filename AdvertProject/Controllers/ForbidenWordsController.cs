using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AdvertProject.Models;

namespace AdvertProject.Controllers
{
    [Authorize]
    public class ForbidenWordsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ForbidenWords
        public ActionResult Index()
        {
            return View(db.ForbidenWords.ToList());
        }

        // GET: ForbidenWords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForbidenWord forbidenWord = db.ForbidenWords.Find(id);
            if (forbidenWord == null)
            {
                return HttpNotFound();
            }
            return View(forbidenWord);
        }

        // GET: ForbidenWords/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ForbidenWords/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Content")] ForbidenWord forbidenWord)
        {
            if (ModelState.IsValid)
            {
                db.ForbidenWords.Add(forbidenWord);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(forbidenWord);
        }

        // GET: ForbidenWords/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForbidenWord forbidenWord = db.ForbidenWords.Find(id);
            if (forbidenWord == null)
            {
                return HttpNotFound();
            }
            return View(forbidenWord);
        }

        // POST: ForbidenWords/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Content")] ForbidenWord forbidenWord)
        {
            if (ModelState.IsValid)
            {
                db.Entry(forbidenWord).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(forbidenWord);
        }

        // GET: ForbidenWords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ForbidenWord forbidenWord = db.ForbidenWords.Find(id);
            if (forbidenWord == null)
            {
                return HttpNotFound();
            }
            return View(forbidenWord);
        }

        // POST: ForbidenWords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ForbidenWord forbidenWord = db.ForbidenWords.Find(id);
            db.ForbidenWords.Remove(forbidenWord);
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
