using ProjectAdvert.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdvertProject.Models
{
    public class AdvertCategory
    {

        public int AdvertCategoryID { get; set; }

        public int AdvertID { get; set; }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }

        public virtual Advert Advert { get; set; }
    }
}