using SocialMarketplace.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class Response
    {
        [Key]
        public int Id { get; set; }
        public String Description { get; set; }
        [Required]
        public ResponseStatus Status { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public Request Request { get; set; }
        [Required, Column("User_Id")]
        public int UserId { get; set; }
        public virtual ICollection<ResponseItem> Items { get; set; }

    }
}