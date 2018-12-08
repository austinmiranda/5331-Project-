using SocialMarketplace.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class SearchFormViewModel
    {
        public SelectList Categories { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public int NumberPages { get; set; }

        public int CategoryId { get; set; }

        public String Keywords { get; set; }

        public String Query { get; set; }

        public int Quantity { get; set; }

        public IList<RequestViewModel> Requests { get; set; }

    }
}