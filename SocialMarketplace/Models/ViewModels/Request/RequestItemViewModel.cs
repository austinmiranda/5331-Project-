using SocialMarketplace.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestItemViewModel
    {
        public int Id { get; set; }
        [StringLength(100), Required(ErrorMessage = "Title is required"), DisplayName("Title:")]
        public String Title { get; set; }
        [StringLength(300), Required(ErrorMessage = "Detail is required"), DisplayName("Details:")]
        public String Detail { get; set; }
        [Required(ErrorMessage = "Select the item type"), DisplayName("Item Type:")]
        public RequestItemType Type { get; set; }
        [Required(ErrorMessage = "Quantity is required"), DisplayName("Quantity:")]
        public int Quantity { get; set; }
    }
}