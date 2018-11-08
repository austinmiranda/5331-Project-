using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Home
{
    public class RequestViewModel
    {
        
        public int Id { get; set; }
        public String Title { get; set; }
        public String Subtitle { get; set; }
        public String Photo { get; set; }
        public int Progress { get; set; }

        public int CategoryId { get; set; }
    }
}