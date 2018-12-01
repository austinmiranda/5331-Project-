using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.ViewModels.Request;
using SocialMarketplace.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SocialMarketplace.Models.Utils;

namespace SocialMarketplace.Controllers.Mobile
{
    public class MobileDonationController : BaseApiController
    {
        private readonly DonationBLO donationBLO = new DonationBLO();

        [HttpGet]
        public DetailViewModel Detail(int id)
        {
            try
            {
                var detailViewModel = donationBLO.GetRequest(id);

                if (detailViewModel == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);

                return detailViewModel;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost, ApiAuthorize]
        public ResponseViewModel Donate(ResponseViewModel responseViewModel)
        {
            try
            {
                donationBLO.SaveResponse(UserId, responseViewModel);
                return responseViewModel;
            }
            catch(Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}