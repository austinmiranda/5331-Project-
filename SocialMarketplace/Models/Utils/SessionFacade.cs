using SocialMarketplace.Models.Entities;
using SocialMarketplace.Models.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.Utils
{
    public static class SessionFacade
    {
        const String REQUEST_STEPS = "RequestSteps";
        const String USER = "User";
        const String AREA = "Area";

        public static RequestStepsViewModel RequestSteps {
            get
            {
                return (RequestStepsViewModel)HttpContext.Current.Session[REQUEST_STEPS];
            }

            set
            {
                HttpContext.Current.Session[REQUEST_STEPS] = value;
            }
        }

        public static System.Security.Principal.IPrincipal User
        {
            get
            {
                return (System.Security.Principal.IPrincipal)HttpContext.Current.Session[USER];
            }

            set
            {
                HttpContext.Current.Session[USER] = value;
            }
        }

        public static int AreaId
        {
            get
            {
                return 1;
                //return (int)HttpContext.Current.Session[AREA];
            }

            set
            {
                HttpContext.Current.Session[AREA] = value;
            }
        }

        public static String RootUrl
        {
            get
            {
                return string.Format("{0}://{1}",
                    HttpContext.Current.Request.Url.Scheme,
                    HttpContext.Current.Request.Url.Authority);
            }
        }
    }
}