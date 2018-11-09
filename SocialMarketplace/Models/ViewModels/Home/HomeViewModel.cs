using SocialMarketplace.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Home
{
    public class HomeViewModel
    {
        public StatisticsViewModel Statistics { get; set; }

        public IList<CategoryViewModel> Categories { get; set; }

        public RequestViewModel MainRequest { get; set; }

        public IList<RequestViewModel> CategoryRequests { get; set; }

        public IList<RequestViewModel> UrgentRequests { get; set; }

        public int? CategoryId { get; internal set; }
    }
}