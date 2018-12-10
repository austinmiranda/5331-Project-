using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using SocialMarketplace.Models;
using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.DAL;

using SocialMarketplace.Models.ViewModels.Request;
using SocialMarketplace.Models.ViewModels.Response;

using SocialMarketplace.Models.Utils;
using SocialMarketplace.Models.ViewModels.Charts;
using System.Web.Helpers;

namespace SocialMarketplace.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        //DonationBLO instance
        private readonly DonationBLO donationBLO = new DonationBLO();
        private readonly DonationGivenBLO donationGivenBLO = new DonationGivenBLO();

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId<int>();
            var user = await UserManager.FindByIdAsync(userId);

            //Console.WriteLine(userId);
            var model = new IndexViewModel
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId.ToString())
            };

            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId<int>(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId<int>(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId<int>(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId<int>(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId<int>(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId<int>(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId<int>(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId<int>());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId<int>());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId<int>(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        //Edit User Profime Get and Post

        public async Task<ActionResult> Edit(int id)
        {

            var user = await UserManager.FindByIdAsync(id);


            return View(new EditUserViewModel()
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Company = user.Company
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include =
    "Id,FirstName,LastName,PhoneNumber,Address,Company")]
    EditUserViewModel editUser)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(editUser.Id);
                if (user == null)
                {
                    return HttpNotFound();
                }

                user.FirstName = editUser.FirstName;
                user.FirstName = editUser.FirstName;
                user.PhoneNumber = editUser.PhoneNumber;
                user.Address = editUser.Address;
                user.Company = editUser.Company;
                UserManager.Update(user);

                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something failed.");
            return View();
        }

        //Get Request Details

        //public ActionResult RequestDetails(int id, int uid)
        //{

        //    //var userId = User.Identity.GetUserId<int>();
        //    ViewBag.uid = uid;
        //    var viewModel = donationBLO.RequestDetails(id, uid);

        //    return View(viewModel);
        //}



        public async Task<ActionResult> RequestDetails(int id, int uid)
        {

            ViewBag.uid = uid;
            using (var context = new ApplicationContext())
            {

                    var request = context.Requests
                .Where(x => x.Id == id && x.UserId == uid).SingleOrDefault();

                    if (request == null)
                        return null;

                    var detailViewModel = new DetailViewModel
                    {
                        Id = request.Id,
                        CategoryId = request.Category.Id,
                        CategoryName = request.Category.Name,
                        Title = request.Title,
                        Subtitle = request.Subtitle,
                        Description = request.Description,
                        DateDue = request.DateDue,
                        Keywords = request.Keywords,
                        VideoURL = request.VideoURL,
                        Progress = request.Progress,
                        Items = new List<RequestItemViewModel>()
                    };

                    foreach (var item in request.Items)
                    {
                        detailViewModel.Items.Add(new RequestItemViewModel
                        {
                            Id = item.Id,
                            Title = item.Title,
                            Detail = item.Detail,
                            Type = item.Type,
                            Quantity = item.Quantity
                        });
                    };

                    var responses = context.Responses
                        .Where(x => x.Request.Id == id)
                        .Select(x => new ResponseUserViewModel
                        {
                            Id = x.Id,
                            Description = x.Description,
                            userId = x.UserId,
                            DateCreated = x.DateCreated
                        // userName = identityC.Users.Where(u => u.Id == x.UserId).SingleOrDefault().;

                    }).ToList();

                    foreach (var r in responses)
                    {
                        var resI = context.ResponseItems.Where(y => y.Response.Id == r.Id).ToList();
                        r.Items = new List<ResponseItemViewModel>();
                        r.userName = new string(new char[] { });
                        var userId = r.userId;
                        var user = await UserManager.FindByIdAsync(userId);
                        System.Diagnostics.Debug.WriteLine(user);
                        if (user == null)
                            r.userName = "Anonymous";
                        else
                            r.userName = user.UserName;

                        foreach (var item in resI)
                        {
                            r.Items.Add(new ResponseItemViewModel
                            {
                                RequestItemName = item.RequestItem.Title,
                                Quantity = item.Quantity
                            });
                        }

                    };

                    var requestDetailsViewModel = new RequestDetailsViewModel
                    {
                        Request = detailViewModel,
                        Responses = responses
                    };


                    return View(requestDetailsViewModel);

            }
        }

        public ActionResult RequestDisable(int id, int uid)
        {
            using(var context = new ApplicationContext())
            {
                var request = context.Requests
                .Where(x => x.Id == id && x.UserId == uid).SingleOrDefault();

                if (request == null)
                    return null;

                if (request.Status == Models.Entities.Enum.RequestStatus.ACTIVE)
                {
                    request.Status = Models.Entities.Enum.RequestStatus.SUSPENDED;
                }
                else if(request.Status == Models.Entities.Enum.RequestStatus.SUSPENDED)
                {
                    request.Status = Models.Entities.Enum.RequestStatus.ACTIVE;
                }

                context.SaveChanges();
            }


            return RedirectToAction("DonationRequested", new { id = uid, sortOrder = "Title" });
        }


            //Get User Requests
            public async Task<ActionResult> DonationRequested(int id, String sortOrder, int? page)
        {

            var user = await UserManager.FindByIdAsync(id);
            var viewModel = donationBLO.GetRequests(id,sortOrder);

            ViewBag.sortType = sortOrder;
            ViewBag.uid = id;


            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(viewModel.ToPagedList(pageNumber, pageSize));
        }


        public ActionResult ViewsChart(int id)
        {

            using(var context = new ApplicationContext())
            {
                var data = context.Requests.Where(r => r.UserId == id && r.Status != Models.Entities.Enum.RequestStatus.IN_PROGRESS)
                    .Select(x => new RequestChartViewModel
                    {
                        
                        Title = x.Title,
                        Views = x.VisualizationCount


                    }).ToList();

                var myChart = new Chart(width: 600, height: 400)
                       .AddTitle("Page Views for Your Requests")
                       .DataBindTable(dataSource: data, xField: "Title")
                       .Write();
                return File(myChart.ToWebImage().GetBytes(), "image/jpeg");
            }

            
        }


        public ActionResult DonationGiven(int id)
        {
            try
            {
                var viewModel = donationGivenBLO.GetResponseList(id);

                return View(viewModel);

            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }

        }

        public ActionResult DonationGivenDetailList(int id)
        {
            try
            {
                var viewModel = donationGivenBLO.GetItemList(id);

                return View(viewModel);

            }
            catch (Exception ex)
            {
                ErrorHandling.AddModelError(ModelState, ex);
                return View();
            }

        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }




        #endregion
    }
}
