using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AdvertProject.Models;


namespace WebApplication19.Controllers
{
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {
        // GET: Roles
        public string Create()
        {
            IdentityManager im = new IdentityManager();

            im.CreateRole("Admin");
            im.CreateRole("User");

            return "OK";
        }

        public string AddToRole()
        {
            IdentityManager im = new IdentityManager();

            im.AddUserToRoleByUsername("b.ciereszynski@wp.pl", "Admin");

            return "OK";
        }
    }
}