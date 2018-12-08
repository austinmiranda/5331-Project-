using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.ViewModels.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Controllers
{
    public class AdminController : Controller
    {
        private AdminBLO adminBLO = new AdminBLO();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DashboardNewUsers()
        {
            try
            {
                var activeUsersPerDay = adminBLO.CalculateActiveUsersPerDay();

                return View(activeUsersPerDay);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(ex.Message, ex);
                return View();
            }
        }

        public ActionResult DashboardDonations()
        {
            try
            {
                var donations = adminBLO.CalculateDonationsPerDay();

                return View(donations);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, ex);
                return View();
            }
        }

        public ActionResult DashboardResponses()
        {
            try
            {
                var donations = adminBLO.CalculateResponsesPerDay();

                return View(donations);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(ex.Message, ex);
                return View();
            }
        }
    }
}
