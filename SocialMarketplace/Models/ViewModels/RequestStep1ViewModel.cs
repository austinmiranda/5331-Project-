using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.ViewModels
{
    public class RequestStep1ViewModel
    {
        public int? CategoryId { get; set; }
        public String Title { get; set; }
        public String Subtitle { get; set; }
        public String Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DueDate { get; set; }

    }
}