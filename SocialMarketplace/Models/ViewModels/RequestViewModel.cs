using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.ViewModels
{
    public class RequestViewModel
    {
        public SelectList Categories { get; set; }
        public RequestStep1ViewModel Step1 { get; set; }
    }
}