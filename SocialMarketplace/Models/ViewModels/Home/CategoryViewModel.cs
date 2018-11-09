using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Home
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public bool Selected { get; set; }
    }
}