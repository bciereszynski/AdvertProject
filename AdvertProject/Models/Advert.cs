using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectAdvert.Models
{
    public class Advert
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Autor")]
        [Required]
        public string Autor { get; set; }

        [Display(Name = "Ogłoszenie")]
        [Required]
        public string Content { get; set; }

        [Display(Name = "Data")]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        public ICollection<Tag> Tags { get; set; }

    }
}