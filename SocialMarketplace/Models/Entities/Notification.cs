using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class Notification
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public Category InterestedInCategory { get; set; }
        [Required, Column("User_Id")]
        public int UserId { get; set; }
        [Required]
        public DateTime DateRequested { get; set; }
    }
}