using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.Utils
{
    public static class ErrorHandling
    {
        public static void AddModelError(ModelStateDictionary model, Exception ex)
        {
            model.AddModelError(string.Empty, ex.Message);
        }
    }
}