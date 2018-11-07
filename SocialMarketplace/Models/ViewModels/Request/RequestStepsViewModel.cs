using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestStepsViewModel
    {
        public int RequestId { get; set; }
        public RequestStep1ViewModel Step1 { get; set; }
        public RequestStep2ViewModel Step2 { get; set; }
        public RequestStep3ViewModel Step3 { get; set; }            
    }
}