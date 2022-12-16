using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ProjectAdvert.Models
{
    public class MyDbContext : DbContext
    {
        public MyDbContext() : base("DBconntection") { }

        public DbSet<Advert> Adverts { get; set; }
    }
}