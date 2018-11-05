using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public String Description { get; set; }
        [StringLength(300)]
        public String Photo { get; set; }
        [StringLength(300)]
        public String VideoURL { get; set; }
        [Required]
        public Request Request { get; set; }
    }
}