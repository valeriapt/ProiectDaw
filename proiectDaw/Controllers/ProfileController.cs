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
			{   // TODO : profile pic ca la pictures asta e la edit nu la show
				var albums = from albm in db.Albums where albm.UserId == userid orderby albm.Name descending select albm;
				
				var pics = from pic in db.Pictures where pic.UserId == userid select pic;
				
				
				/*
				ViewBag.Albums = albums;
				var pics = from pic in db.Pictures where pic.UserId == userid select pic;
				ViewBag.Pictures = pics;
				ViewBag.Profile = profile;
				*/
				return View();
				

			}
		}
    }
}