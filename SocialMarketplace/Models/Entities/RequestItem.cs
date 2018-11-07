using SocialMarketplace.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class RequestItem
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100), Required]
        public String Title { get; set; }
        [StringLength(300), Required]
        public String Detail { get; set; }
        [Required]
        public RequestItemType Type { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public Request Request { get; set; }
    }
}