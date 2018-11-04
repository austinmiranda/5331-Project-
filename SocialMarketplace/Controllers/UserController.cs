using SocialMarketplace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Controllers
{
    public class UserController : Controller
    {
        public ActionResult SignIn()
        {
            return View();
        }

      
        public ActionResult Register()
        {
           
                return View();
            
        }

        public ActionResult UserProfile()
        {
            return View();
        }
    }
}
