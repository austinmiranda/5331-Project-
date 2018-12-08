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
using SocialMarketplace.Models.ViewModels.Home;

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

        [HttpGet, ApiAuthorize]
        public RequestListViewModel MyRequests(int skip, int take)
        {
            try
            {
                return donationBLO.MyRequests(UserId, skip, take);
            }
            catch(Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public SearchResultViewModel Search(SearchViewModel searchViewModel, int skip, int take)
        {
            try
            {
                return donationBLO.Search(searchViewModel, skip, take);
            }
            catch(Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost, ApiAuthorize]
        public void Question(QuestionViewModel questionViewModel)
        {
            try
            {
                donationBLO.SaveQuestion(UserId, questionViewModel);
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

        [HttpPost, ApiAuthorize]
        public RequestStepsViewModel SaveRequest(RequestStepsViewModel request)
        {
            try
            {
                donationBLO.SaveRequest(UserId, request.Step1.RequestInForm, request.Step2.Items, request.Step3.RequestOptional);
                return request;
            }
            catch(Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut, ApiAuthorize]
        public void SaveRequestPhoto([FromUri] int requestId)
        {
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files.Count > 0 ?
                    HttpContext.Current.Request.Files[0] : null;

                if (file != null && file.ContentLength > 0)
                {
                    donationBLO.SaveRequestPhoto(UserId, requestId, file);
                }
            }
            catch (Exception ex)
            {
                var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                responseMessage.Content = new StringContent(ex.Message + " " + ex.StackTrace.ToString());
                throw new HttpResponseException(responseMessage);
            }
        }
    }
}