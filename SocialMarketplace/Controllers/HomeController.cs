using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Entities;
using SocialMarketplace.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Controllers
{
    public class HomeController : Controller
    {
        private readonly HomeBLO homeBLO = new HomeBLO();

        public ActionResult Index(int? id)
        {
            var viewModel = homeBLO.CreateHomeViewModel(id);

            return View(viewModel);
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