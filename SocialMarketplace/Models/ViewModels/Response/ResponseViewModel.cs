using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Response
{
    public class ResponseViewModel
    {
        public int? Id { get; set; }
        public String Description { get; set; }
        public int RequestId { get; set; }
        public IList<ResponseItemViewModel> Items { get; set; }
    }
}