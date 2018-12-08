using SocialMarketplace.Models.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Response
{
    public class ResponseFormViewModel
    {
        public int RequestId { get; set; }

        public string Description { get; set; }

        public IList<ResponseItemFormViewModel> Items { get; set; }
    }
}