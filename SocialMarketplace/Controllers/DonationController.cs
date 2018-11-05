using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.ViewModels;

namespace SocialMarketplace.Controllers
{
    public class DonationController : Controller
    {
        private readonly DonationBLO donationBLO = new DonationBLO();

        public ActionResult RequestStep1()
        {
            var viewModel = donationBLO.CreateEmptyDonationViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult RequestStep1(RequestViewModel request)
        {
            // TODO: server validation
            // TODO: exception handling

            if(donationBLO.SaveStep1Request(request))
            {
                return View("RequestStep2", request);
            }
            else
            {
                // TODO: error handling

                request.Categories = donationBLO.GetCategories();
                return View(request);
            }
        }

        [HttpPost]
        public ActionResult RequestStep2()
        {
            return View();
        }
        public ActionResult RequestStep3()
        {
            return View();
        }
        public ActionResult FinishAskForDonation()
        {
            return View();
        }
        public ActionResult Response()
        {
            return View();
        }

        public ActionResult Completed()
        {
            return View();
        }

        public ActionResult Search()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Result()
        {
            return View();
        }

        public ActionResult Detail()
        {
            return View();
        }

        public ActionResult DetailCompleted()
        {
            return View();
        }

    }
}