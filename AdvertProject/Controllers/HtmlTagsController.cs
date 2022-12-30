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
    public class HtmlTagsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: HtmlTags
        public ActionResult Index()
        {
            return View(db.HtmlTags.ToList());
        }
        

        // GET: HtmlTags/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HtmlTags/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Tag")] HtmlTag htmlTag)
        {
            if (ModelState.IsValid)
            {
                db.HtmlTags.Add(htmlTag);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(htmlTag);
        }

      
        // GET: HtmlTags/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HtmlTag htmlTag = db.HtmlTags.Find(id);
            if (htmlTag == null)
            {
                return HttpNotFound();
            }
            return View(htmlTag);
        }

        // POST: HtmlTags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            HtmlTag htmlTag = db.HtmlTags.Find(id);
            db.HtmlTags.Remove(htmlTag);
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
