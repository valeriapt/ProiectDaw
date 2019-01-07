using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using proiectDaw.Models;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace proiectDaw.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        private ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index()
        {
            String userid = User.Identity.GetUserId();
			var profile = from profileUser in db.Profiles where profileUser.UserId == userid select profileUser;
			if (profile == null)
				return RedirectToAction("Edit");
			else
			{   // TODO : profile pic ca la pictures
				ViewBag.Profile = profile;
				return View();
			}
        }
    }
}