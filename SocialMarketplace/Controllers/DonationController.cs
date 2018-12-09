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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using SocialMarketplace.Models.ViewModels.Response;

namespace SocialMarketplace.Controllers
{
    public class DonationController : BaseController
    {
        private readonly int POSITION_KEY = 11;
        
        private readonly DonationBLO donationBLO = new DonationBLO();

        [Authorize]
        public ActionResult RequestStep1(int? id)
        {
            try
            {
                var viewModel = SessionFacade.RequestSteps;

                if(viewModel == null)
                {
                    viewModel = donationBLO.CreateEmptyDonationViewModel();
                    SessionFacade.RequestSteps = viewModel;
                }

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
                    donationBLO.SaveRequest(User.Identity.GetUserId<int>(), viewModel.Step1.RequestInForm);
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
                
                if(request.Delete > 0)
                {
                    donationBLO.DeleteItem(viewModel.RequestId, request.Delete.Value);
                    viewModel.Step2.Items.Remove(viewModel.Step2.Items.Where(x => x.Id == request.Delete.Value).Single());
                    ModelState.Clear();
                    return View(viewModel.Step2);
                }

                if(request.Command.Equals("Back"))
                {
                    return RedirectToAction("RequestStep1");
                }

                if (ModelState.IsValid)
                {
                    donationBLO.AddRequestItem(viewModel.Step2.ItemInForm, viewModel.RequestId);
                    viewModel.Step2.Items.Add(viewModel.Step2.ItemInForm);
                    viewModel.Step2.ItemInForm = new RequestItemViewModel();

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
                if (request.Command.Equals("Back"))
                {
                    return RedirectToAction("RequestStep2");
                }

                viewModel.Step3.RequestOptional = request.RequestOptional;

                if (ModelState.IsValid)
                {
                    donationBLO.SaveRequest(User.Identity.GetUserId<int>(), viewModel.Step1.RequestInForm, requestOptional: viewModel.Step3.RequestOptional);
                    SessionFacade.RequestSteps = null;

                    return RedirectToAction("FinishAskForDonation");
                }

                return View(viewModel.Step3);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View(viewModel.Step3);
            }
        }

        [Authorize]
        public ActionResult Duplicate(int id)
        {
            try
            {
                donationBLO.Duplicate(User.Identity.GetUserId<int>(), id);

                return RedirectToAction("DuplicateSuccess");
            }
            catch(Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        public ActionResult DuplicateSuccess()
        {
            return View();
        }

        [Authorize]
        public ActionResult FinishAskForDonation()
        {
            return View();
        }

        public ActionResult Photo(int id)
        {
            var response = new HttpResponseMessage();

            try
            {
                byte[] photo = donationBLO.Photo(id);

                if(photo != null)
                {
                    return File(photo, "image/png");
                }
                else
                    return HttpNotFound();
            }
            catch (Exception)
            {
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
        }

        [Authorize]
        public ActionResult ResponseFor(int id)
        {
            try
            {
                ResponseFormViewModel response = donationBLO.CreateEmptyResponseViewModel(id);

                return View(response);
            }
            catch(Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        [ValidateAntiForgeryToken]
        [Authorize, HttpPost]
        public ActionResult ResponseFor(ResponseFormViewModel response)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var responseViewModel = new ResponseViewModel
                    {
                        Description = response.Description,
                        RequestId = response.RequestId,
                        Items = GetResponseItems(Request)
                    };

                    donationBLO.SaveResponse(User.Identity.GetUserId<int>(), responseViewModel);
                    return RedirectToAction("Completed");
                }
                else
                    return View(response);
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View(donationBLO.CreateEmptyResponseViewModel(response.RequestId));
            }
        }

        private IList<ResponseItemViewModel> GetResponseItems(HttpRequestBase request)
        {
            var items = new List<ResponseItemViewModel>();

            foreach(var item in request.Form.AllKeys)
            {
                if(item.StartsWith("IsDonating_"))
                {
                    string key = item.Substring(POSITION_KEY);
                    int requestItemId = int.Parse(key);
                    int quantity = int.Parse(request.Form["RequestItem_" + key]);

                    items.Add(new ResponseItemViewModel
                    {
                        RequestItemId = requestItemId,
                        Quantity = quantity
                    });
                }
            };

            return items;
        }

        public ActionResult Completed()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        public ActionResult Detail(int id)
        {
            try
            {
                var detailViewModel = donationBLO.GetRequest(id);

                if (detailViewModel == null)
                    return HttpNotFound();

                ViewBag.Title = detailViewModel.Title;

                return View(detailViewModel);
            }
            catch(Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Detail(int id, String command)
        {
            try
            {
                if(command.Equals("Question"))
                    return RedirectToAction("Question", new { id });

                if(command.Equals("Donate"))
                    return RedirectToAction("ResponseFor", new { id });

                if (command.Equals("Duplicate"))
                    return RedirectToAction("Duplicate", new { id });

                if (command.Equals("Notification"))
                    return RedirectToAction("AddNotification", new { id });

                return View();
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        [Authorize]
        public ActionResult AddNotification(int id)
        {
            try
            {
                donationBLO.AddNotification(User.Identity.GetUserId<int>(), id);

                return RedirectToAction("AddNotificationSuccess");
            }
            catch(Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        public ActionResult AddNotificationSuccess()
        {
            return View();
        }

        public ActionResult Question(int id, String result)
        {
            try
            {
                ViewBag.Result = result;
                return View(donationBLO.CreateEmptyQuestionViewModel(id));
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Question(QuestionViewModel viewModel)
        {
            try
            {
                donationBLO.SaveQuestion(User.Identity.GetUserId<int>(), viewModel);
                return RedirectToAction("Question", 
                    new { id = viewModel.RequestId, result = "Thank you for your question." });
            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }
        }

        public ActionResult DetailCompleted()
        {
            return View();
        }



        
        //Detils User request Get 

        public ActionResult RequestDetails(int id, int uid)
        {

            //var userId = User.Identity.GetUserId<int>();
            ViewBag.uid = uid;
            var viewModel = donationBLO.RequestDetails(id,uid);

            return View(viewModel);
        }

    }
}