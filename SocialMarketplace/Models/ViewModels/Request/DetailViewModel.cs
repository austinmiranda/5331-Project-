using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class DetailViewModel
    {
        public int? Id { get; set; }
        public int? CategoryId { get; set; }
        public String CategoryName { get; set; }
        public String Title { get; set; }
        public String Subtitle { get; set; }
        public String Description { get; set; }
        public DateTime DateDue { get; set; }
        public String Keywords { get; set; }
        public String PhotoURL { get; set; }
        public String VideoURL { get; set; }
        public int Progress { get; set; }

        public IList<RequestItemViewModel> Items { get; set; }
    }
}