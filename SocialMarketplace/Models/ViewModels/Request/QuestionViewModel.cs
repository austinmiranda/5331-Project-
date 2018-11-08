using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class QuestionViewModel
    {
        public int RequestId { get; set; }
        public String Title { get; set; }
        public String Subtitle { get; set; }

        [StringLength(4000), Required(ErrorMessage = "Question is required"), DisplayName("Question:")]
        public String Description { get; set; }
    }
}