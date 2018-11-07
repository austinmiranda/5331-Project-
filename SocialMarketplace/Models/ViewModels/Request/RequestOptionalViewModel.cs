using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SocialMarketplace.Models.ViewModels.Request
{
    public class RequestOptionalViewModel
    {
        [StringLength(300), DisplayName("Keywords:")]
        public String Keywords { get; set; }

        [DisplayName("Photo:")]
        public HttpPostedFileBase Photo { get; set; }

        [StringLength(300), DisplayName("Video URL:")]
        public String VideoURL { get; set; }
    }
}