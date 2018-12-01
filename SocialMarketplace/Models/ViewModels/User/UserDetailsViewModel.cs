using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.User
{
    public class UserDetailsViewModel
    {
        public String UserName { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String PhoneNumber { get; set; }

        public String Email { get; set; }

        public String Address { get; set; }

        public String Company { get; set; }
    }
}