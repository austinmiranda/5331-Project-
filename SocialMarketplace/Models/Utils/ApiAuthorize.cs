using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using SocialMarketplace.Controllers;

namespace SocialMarketplace.Models.Utils
{
    public class ApiAuthorize : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            bool isAuthorized = base.IsAuthorized(actionContext);

            try
            {
                if (!isAuthorized)
                    return false;

                var request = actionContext.Request;
                var context = request.GetOwinContext();

                var claims = ((Microsoft.Owin.OwinContext)context).Authentication.User.Claims;
                string value = claims.Where(x => x.Type == "id").SingleOrDefault().Value;
                int id = int.Parse(value);

                if(actionContext.ControllerContext.Controller is BaseApiController)
                {
                    var controller = (BaseApiController)actionContext.ControllerContext.Controller;
                    controller.UserId = id;
                }                

                return true;
            }
            catch(Exception)
            {
                // Keep the default behaviour
                return false;
            }
        }
    }
}