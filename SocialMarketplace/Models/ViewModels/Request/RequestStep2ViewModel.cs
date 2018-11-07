using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestStep2ViewModel
    {
        public SelectList RequestItemTypes { get; set; }
        public RequestItemViewModel ItemInForm { get; set; }

        public IList<RequestItemViewModel> Items { get; set; }
        public String Command { get; set; }
        public int? Delete { get; set; }
    }
}