﻿using SocialMarketplace.Models.BLL;
using SocialMarketplace.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace SocialMarketplace.Controllers.Mobile
{
    public class MobileHomeController : ApiController
    {
        private readonly HomeBLO homeBLO = new HomeBLO();

        [HttpGet]
        public HomeViewModel Get(int? id)
        {
            try
            {
                var viewModel = homeBLO.CreateHomeViewModel(id);

                return viewModel;
            }
            catch (Exception)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
