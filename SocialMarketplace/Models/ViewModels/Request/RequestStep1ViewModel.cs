using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestStep1ViewModel
    {
        public SelectList Categories { get; set; }
        public RequestViewModel RequestInForm { get; set; }
    }
}