using SocialMarketplace.Models.ViewModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestDetailsViewModel
    {
        public DetailViewModel Request { get; set; }
        public IList<ResponseUserViewModel> Responses { get; set; }

    }
}