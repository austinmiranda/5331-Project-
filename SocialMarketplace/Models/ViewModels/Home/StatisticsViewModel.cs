using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Home
{
    public class StatisticsViewModel
    {
        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int TotalDonors { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int TotalDonations { get; set; }

        [DisplayFormat(DataFormatString = "{0:N0}")]
        public int ActiveDonations { get; set; }
    }
}