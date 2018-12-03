using SocialMarketplace.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class SearchViewModel
    {
        public int CategoryId { get; set; }

        public String Keywords { get; set; }

        public String Query { get; set; }
    }
}