using SocialMarketplace.Models.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "Category is required"), DisplayName("Category:")]
        public int CategoryId { get; set; }

        [DisplayName("Keywords:")]
        public String Keywords { get; set; }

        [DisplayName("Query:")]
        public String Query { get; set; }

        public int Quantity { get; set; }

        public IList<RequestViewModel> Requests { get; set; }

    }
}