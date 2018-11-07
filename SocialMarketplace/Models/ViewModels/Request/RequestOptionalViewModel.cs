using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestOptionalViewModel
    {
        public String Keywords { get; set; }
        public HttpPostedFileBase Photo { get; set; }
        public String VideoURL { get; set; }
    }
}