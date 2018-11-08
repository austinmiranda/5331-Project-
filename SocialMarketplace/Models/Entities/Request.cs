using SocialMarketplace.Models.Entities.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class Request
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100), Required]
        public String Title { get; set; }

        [StringLength(300), Required]
        public String Subtitle { get; set; }

        [Required]
        public String Description { get; set; }

        [StringLength(300)]
        public String Keywords { get; set; }

        public byte[] Photo { get; set; }

        [StringLength(300)]
        public String VideoURL { get; set; }

        [Required]
        public int VisualizationCount { get; set; }

        [Required]
        public int Progress { get; set; }

        [Required]
        public RequestStatus Status { get; set; }

        [Required]
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DateDue { get; set; }

        [Required]
        public virtual Category Category { get; set; }

        [Required, Column("User_Id")]
        public int UserId { get; set; }

        [Required]
        public virtual Area Area { get; set; }
        public virtual ICollection<RequestItem> Items { get; set; }
    }
}