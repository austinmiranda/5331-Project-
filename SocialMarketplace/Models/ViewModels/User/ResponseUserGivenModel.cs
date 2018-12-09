using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.User
{
    public class ResponseUserGivenModel
    {
        public int Id { get; set; }
        public String RequestTitle { get; set; }
        public String Description { get; set; }
        public DateTime DataCreated { get; set; }
    }
}