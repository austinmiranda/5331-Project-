using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using SocialMarketplace.Models;
using SocialMarketplace.Models.Utils;
using SocialMarketplace.Models.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SocialMarketplace.Controllers.Mobile
{
    public class MobileAccountController : BaseApiController
    {
        private AuthRepository _repo = null;

        public MobileAccountController()
        {
            _repo = new AuthRepository();
        }

        // POST api/Account/Register
        [HttpPost, AllowAnonymous]
        public async Task<IHttpActionResult> Register(RegisterViewModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _repo.RegisterUser(userModel);

            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        [HttpGet, ApiAuthorize]
        public UserDetailsViewModel GetInfo()
        {
            var user = _repo.FindUser(UserId);

            return new UserDetailsViewModel
            {
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                Company = user.Company
            };
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
