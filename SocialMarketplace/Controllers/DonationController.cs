using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Utils;
using SocialMarketplace.Models.ViewModels.Request;
using Microsoft.AspNet.Identity;
using System.Web.Routing;

namespace SocialMarketplace.Controllers
{
    public class DonationController : BaseController
    {
        private readonly DonationBLO donationBLO = new DonationBLO();

        [Authorize]
        public ActionResult RequestStep1()
        {
            try
            {
                var viewModel = donationBLO.CreateEmptyDonationViewModel();
                SessionFacade.RequestSteps = viewModel;

                return View(viewModel.Step1);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        [Authorize, HttpPost]
        public ActionResult RequestStep1(RequestStep1ViewModel request)
        {
            var viewModel = SessionFacade.RequestSteps;

            try
            {
                viewModel.Step1.RequestInForm = request.RequestInForm;

                if (ModelState.IsValid)
                {
                    donationBLO.SaveRequest(viewModel.Step1.RequestInForm);
                    viewModel.RequestId = viewModel.Step1.RequestInForm.Id.Value;
                    return RedirectToAction("RequestStep2");
                }
                else
                    return View(viewModel.Step1);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View(viewModel.Step1);
            }
        }

        [Authorize]
        public ActionResult RequestStep2()
        {
            var viewModel = SessionFacade.RequestSteps;

            try
            {
                return View(viewModel.Step2);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View(viewModel.Step2);
            }
        }

        [ValidateAntiForgeryToken]
        [Authorize, HttpPost]
        public ActionResult RequestStep2(RequestStep2ViewModel request)
        {
            var viewModel = SessionFacade.RequestSteps;

            try
            {
                viewModel.Step2.ItemInForm = request.ItemInForm;
                
                if (ModelState.IsValid)
                {
                    donationBLO.AddRequestItem(viewModel.Step2.ItemInForm, viewModel.RequestId);
                    viewModel.Step2.Items.Add(viewModel.Step2.ItemInForm);

                    if(request.Command.Equals("AddMore"))
                    {
                        viewModel.Step2.ItemInForm = new RequestItemViewModel();
                        return RedirectToAction("RequestStep2");
                    }
                }

                if (request.Command.Equals("Next") && viewModel.Step2.Items.Count > 0)
                    return RedirectToAction("RequestStep3");
                else
                    return View(viewModel.Step2);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View(viewModel.Step2);
            }
        }

        [Authorize]
        public ActionResult RequestStep3()
        {
            var viewModel = SessionFacade.RequestSteps;

            try
            {
                return View(viewModel.Step3);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View(viewModel.Step3);
            }
        }

        [ValidateAntiForgeryToken]
        [Authorize, HttpPost]
        public ActionResult RequestStep3(RequestStep3ViewModel request)
        {
            var viewModel = SessionFacade.RequestSteps;

            try
            {
                viewModel.Step3.RequestOptional = request.RequestOptional;

                if (ModelState.IsValid)
                {
                    donationBLO.SaveRequest(viewModel.Step1.RequestInForm, viewModel.Step3.RequestOptional);

                    return RedirectToAction("FinishAskForDonation");
                }

                return View(viewModel.Step3);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View(viewModel.Step2);
            }
        }

        [Authorize]
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