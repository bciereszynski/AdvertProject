﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdvertProject.Models
{
    public class Category
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }


        [Display(Name = "Kategoria bazowa")]
        public int? RootCategoryID { get; set; }

        [Display(Name = "Kategoria bazowa")]
        public virtual Category RootCategory { get; set; }

        public ICollection<AdvertCategory> adverts { get; set; }
    }
}