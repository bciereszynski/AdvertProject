using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using AdvertProject.Migrations;
using AdvertProject.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json.Linq;
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
            if(TempData["FilterCategories"] != null)
            {
                String categoriesIdsString = Convert.ToString(TempData["FilterCategories"]);
                List<string> choosesList = categoriesIdsString.Split(',').ToList();
                foreach (string category in choosesList)
                {
                    var id = Int32.Parse(category);
                    var cat = db.Categories.Where(x => x.ID == id).FirstOrDefault();
                    adverts = adverts.Where(x => x.categories.Contains(db.AdvertCategories.Where(ac => ac.CategoryID == cat.ID).FirstOrDefault()));
                }
            }
            
            return View(adverts.ToList());
        }

        public ActionResult Filter()
        {
            ViewBag.Categories = new MultiSelectList(db.Categories, "ID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Filter(FormCollection form)
        {
            String categoriesIdsString = form["categories"];
            TempData["FilterCategories"] = categoriesIdsString;
            return RedirectToAction("Index");
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


        private string HtmlTagValidate(string content)
        {
            //Prepere list of possible begining of html tags
            List<int> indexes = new List<int>();
            for (int index = 0; ; index += 1)
            {
                index = content.IndexOf('<', index);
                if (index == -1)    //no more
                    break;
                if (index - 1 >= 0 && content[index - 1] == '\\') //check if escaped
                {
                    content.Remove(index - 1, index - 1); //remove escape sing
                    continue;
                }

                if (index + 1 < content.Length && content[index + 1] != '/')    //chcek if ending
                    indexes.Add(index);
            }

            int HtmlClear(int i)
            {
                var index = indexes[i];
                //two kinds of engings - small e.g <a ... />, big e.g <b>.... </b>
                int nearestSmall = content.IndexOf("/>", index);
                int nearestBig = content.IndexOf("</", index);
                int next;
                if (nearestSmall == -1 && nearestBig == -1)
                    return i;
                if (i + 1 < indexes.Count)
                    next = indexes[i + 1];
                else
                    next = -1;
                //check if allowed
                foreach (var allowedTag in db.HtmlTags.ToList())
                {
                    if (content.Substring(index + 1).StartsWith(allowedTag.Tag))
                    {
                        return i;
                    }
                }

                //check if something inside - recursive
                if (next > -1 && nearestSmall > next && nearestBig > next)
                    i = HtmlClear(i + 1);
               

                if (nearestSmall < next || (next == -1 && nearestSmall != -1))
                {
                    content = content.Remove(index, nearestSmall - index + 1);
                }
                else if (nearestBig < next || (next == -1 && nearestBig != -1))
                {
                    content = content.Remove(index, content.IndexOf(">", index) - index + 1);
                    nearestBig = content.IndexOf("</", index);
                    content = content.Remove(nearestBig, content.IndexOf(">", nearestBig) - nearestBig + 1);
                }
                return i;
            }

            for (int i = 0; i < indexes.Count; i++)
            {
                i = HtmlClear(i);
            }
            return content;

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
                //Content validation in case of forbiden words

                List<ForbidenWord> forbidenWords = db.ForbidenWords.ToList();
                bool contentSafe = true;

                foreach(ForbidenWord f in forbidenWords)
                {
                    if (advert.Content.Contains(f.Content))
                    {
                        contentSafe = false;
                        ModelState.AddModelError("Content", "Ogłoszenie zawiera zakazane słowo: " + f.Content);
                        break;
                    }
                }
                if(contentSafe == true)
                {

                    //HTML Tags review
                    //possible to escape < symbol with '\'
                    //TODO: TESTYH
                    var content = advert.Content;
                    advert.Content = HtmlTagValidate(content);


                    //unvalidated to allow HTML - form is after validation at this point!
                    var form = this.HttpContext.ApplicationInstance.Context.Request.Unvalidated().Form;
                    //Categories insert
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
                    //user equip
                    advert.UserID = User.Identity.GetUserId();
                    advert.User = db.Users.Find(advert.UserID);
                    //date set
                    advert.Date = DateTime.Now;
                    db.Adverts.Add(advert);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
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
        public ActionResult Edit([Bind(Include = "ID,Content,UserID,Date")] Advert advert)
        {
            if (User.Identity.GetUserId() != advert.UserID)
            {
                return RedirectToAction("Index");
            }

            List<string> selectedCategories = null;
            if (ModelState.IsValid)
            {
                var form = this.HttpContext.ApplicationInstance.Context.Request.Unvalidated().Form;
                //save choosed categories
                String categoryIds = form["categories"];
                if (categoryIds != null)
                {
                    selectedCategories = categoryIds.Split(',').ToList();
                }
                else
                {
                    selectedCategories = new List<string>();
                }
                //Content validation in case of forbiden words
                List<ForbidenWord> forbidenWords = db.ForbidenWords.ToList();
                bool contentSafe = true;

                foreach (ForbidenWord f in forbidenWords)
                {
                    if (advert.Content.Contains(f.Content))
                    {
                        contentSafe = false;
                        ModelState.AddModelError("Content", "Ogłoszenie zawiera zakazane słowo: " + f.Content);
                    }
                }
                if (contentSafe == true)
                {
                    //HTML TAGS treatment
                    var content = advert.Content;
                    advert.Content = HtmlTagValidate(content);

                    //Deleting all categories
                    foreach (AdvertCategory ac in db.AdvertCategories.Where(x => x.AdvertID == advert.ID))
                    {
                        db.AdvertCategories.Remove(ac);
                    }
                    //Adding edited categories

                    foreach (string category in selectedCategories)
                    {
                        int categoryId = Int32.Parse(category);
                        AdvertCategory ac = new AdvertCategory();
                        ac.AdvertID = advert.ID;
                        ac.CategoryID = categoryId;
                        db.AdvertCategories.Add(ac);
                    }
                    //Setting new time - TODO: check if needed - mayby modified flag?
                    advert.Date = DateTime.Now;
                    db.Entry(advert).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            //If is true, if advert contains forbidden word
            if(selectedCategories == null)
            {
                advert = db.Adverts.Where(x => x.ID == advert.ID).Include(x => x.categories).FirstOrDefault();
                foreach (AdvertCategory category in advert.categories.ToList())
                {
                    selectedCategories.Add(category.CategoryID.ToString());
                }
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
