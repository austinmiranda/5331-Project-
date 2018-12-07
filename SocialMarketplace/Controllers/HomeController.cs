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
using SocialMarketplace.Models.ViewModels.Request;

namespace SocialMarketplace.Controllers
{
    public class HomeController : BaseController
    {
        private readonly HomeBLO homeBLO = new HomeBLO();
        private readonly DonationBLO donationBLO = new DonationBLO();

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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult Search(SearchFormViewModel search)
        {
            try
            {
                int skip = search.Page * search.PageSize;
                int take = search.PageSize;

                var searchViewModel = new SearchViewModel
                {
                    CategoryId = search.CategoryId,
                    Keywords = search.Keywords,
                    Query = search.Query
                };

                var result = donationBLO.Search(searchViewModel, skip, take);

                search.Requests = result.Requests;
                search.Quantity = result.Quantity;
                search.NumberPages = result.Quantity / search.PageSize;

                if (result.Quantity % search.PageSize > 0)
                    search.NumberPages++;

                return View("Result", search);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        public ActionResult Search()
        {
            try
            {
                var searchform = homeBLO.CreateEmptySearchViewModel();

                return View(searchform);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }
    }
}