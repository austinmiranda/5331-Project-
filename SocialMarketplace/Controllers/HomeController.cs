using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var context = new ApplicationContext())
            {
                context.Categories.Add(new Category { Id = 1, Name = "Groceries" });
                context.SaveChanges();

                return View(context.Categories);
            }
            //return View();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult PrivacyCookiesPolicy()
        {
            return View();
        }

        public ActionResult TermsOfUse()
        {
            return View();
        }
    }
}