using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.Entities
{
    public class Photo
    {
        [Key]
        public int Id { get; set; }
        public byte[] Data { get; set; }
        public Request Request { get; set; }
    }
}