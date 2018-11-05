using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class ResponseItem
    {
        [Key]
        public int Id { get; set; }
        public int Quantity { get; set; }
        public RequestItem RequestItem { get; set; }
        public Response Response { get; set; }
    }
}