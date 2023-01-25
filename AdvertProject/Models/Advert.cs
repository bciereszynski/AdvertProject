using AdvertProject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace ProjectAdvert.Models
{
    [OutputCache(NoStore = true, Duration = 0, Location = OutputCacheLocation.None)]
    public class Advert
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Advert", ResourceType = typeof(Resources.Content))]
        [DataType(DataType.MultilineText)]
        [Required]
        [MaxLength(1500)]
        [AllowHtml]
      
        public string Content { get; set; }

        public string UserID { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Display(Name = "Date", ResourceType = typeof(Resources.Content))]
        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [Display(Name = "Category", ResourceType = typeof(Resources.Content))]
        public ICollection<AdvertCategory> categories{ get; set; }

    }
}