using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Response
{
    public class ResponseItemFormViewModel
    {
        public bool IsDonating { get; set; }

        public ResponseItemViewModel ResponseItemInForm { get; set; }

        public Request.RequestItemViewModel RequestItem { get; set; }
    }
}