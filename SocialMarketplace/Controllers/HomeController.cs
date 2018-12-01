using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Entities;
using SocialMarketplace.Models.Utils;
using SocialMarketplace.Models.ViewModels;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Controllers
{
    public class HomeController : BaseController
    {
        private readonly HomeBLO homeBLO = new HomeBLO();

        public ActionResult Index(int? id)
        {
            try
            {
                var viewModel = homeBLO.CreateHomeViewModel(id);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
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