using SocialMarketplace.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class DonationRequestedViewModel
    {
        public int Id { get; set; }
        public String Title { get; set; }
        public String Subtitle { get; set; }
        public String Description { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateDue { get; set; }
        public RequestStatus Status { get; set; }
        public int Progress { get; set; }
        public string Category { get; set; }
    }
}