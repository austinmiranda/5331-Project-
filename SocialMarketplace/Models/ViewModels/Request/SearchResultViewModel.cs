using SocialMarketplace.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class SearchResultViewModel
    {
        public int Quantity { get; set; }

        public SearchViewModel Attributes { get; set; }

        public IList<RequestViewModel> Requests { get; set; }
    }
}