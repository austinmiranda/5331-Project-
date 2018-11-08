using SocialMarketplace.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class Question
    {
        [Key]
        public int Id { get; set; }

        [StringLength(4000), Required]
        public String Description { get; set; }

        [Required]
        public QuestionType Type { get; set; }

        [Required, Column("User_Id")]
        public int UserId { get; set; }

        [Required]
        public Request Request { get; set; }
    }
}