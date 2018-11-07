using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [StringLength(100), Required]
        public String Name { get; set; }
    }
}