using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace proiectDaw.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        private ApplicationDbContext db = ApplicationDbContext.Create();

        public ActionResult Index()
        {
            String userid = User.Identity.GetUserId();
            var user = db.Users.Find(userid);
            return View(user);
        }
    }
}