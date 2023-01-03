using AdvertProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace AdvertProject.Controllers
{
    public class RssController : Controller
    {
        public class RSSActionResult : ActionResult
        {
            private SyndicationFeed _feed { get; set; }

            public RSSActionResult(SyndicationFeed feed)
            {
                _feed = feed;
            }
            public override void ExecuteResult(ControllerContext context)
            {
                context.HttpContext.Response.ContentType = "application/rss+xml";
                var formatter = new Rss20FeedFormatter(_feed);
                using (var writer = XmlWriter.Create(context.HttpContext.Response.Output))
                {
                    formatter.WriteTo(writer);
                }
            }
        }
        // GET: Rss
        public ActionResult Index()
        {
            var app = HttpContext.Application;
            var feed = (SyndicationFeed)app["RssFeed"];
            if(feed!=null)
                return new RSSActionResult(feed);
            return View();

        }
    }
}