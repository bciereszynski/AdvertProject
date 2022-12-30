using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AdvertProject.Models
{
    public class HtmlTag
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Tag HTML")]
        [Required]
        public string Tag { get; set; }
    }
}