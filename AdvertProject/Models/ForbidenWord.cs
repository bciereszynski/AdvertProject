﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace AdvertProject.Models
{
    public class ForbidenWord
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Słowo zakazane")]
        [Required]
        public string Content { get; set; }
    }
}