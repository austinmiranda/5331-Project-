using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.DAL;
using SocialMarketplace.Models.Utils;
using SocialMarketplace.Models.ViewModels.Request;

namespace SocialMarketplace.Controllers
{
    public class DonationController : Controller
    {
        private readonly DonationBLO donationBLO = new DonationBLO();

        public ActionResult RequestStep1()
        {
            try
            {
                var viewModel = donationBLO.CreateEmptyDonationViewModel();
                Session["viewModel"] = viewModel;

                return View(viewModel.Step1);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        [HttpPost]
        public ActionResult RequestStep1(RequestStep1ViewModel request)
        {
            var viewModel = (RequestStepsViewModel)Session["viewModel"];

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

        public ActionResult RequestStep2()
        {
            var viewModel = (RequestStepsViewModel)Session["viewModel"];

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

        [HttpPost]
        public ActionResult RequestStep2(RequestStep2ViewModel request)
        {
            var viewModel = (RequestStepsViewModel)Session["viewModel"];

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

        public ActionResult RequestStep3()
        {
            var viewModel = (RequestStepsViewModel)Session["viewModel"];

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

        [HttpPost]
        public ActionResult RequestStep3(RequestStep3ViewModel request)
        {
            var viewModel = (RequestStepsViewModel)Session["viewModel"];

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