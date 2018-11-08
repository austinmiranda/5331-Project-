using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestMandatoryViewModel
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Category is required"), DisplayName("Category:")]
        public int? CategoryId { get; set; }

        [StringLength(100), Required(ErrorMessage = "Title is required"), DisplayName("Title:")]
        public String Title { get; set; }

        [StringLength(300), Required(ErrorMessage = "Subtitle is required"), DisplayName("Sub-title:")]
        public String Subtitle { get; set; }

        [Required(ErrorMessage = "Description is required"), DisplayName("Description:")]
        public String Description { get; set; }

        [DataType(DataType.Date), DisplayName("Due Date:"), Required(ErrorMessage = "Due Date is required")]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateDue { get; set; }
    }
}