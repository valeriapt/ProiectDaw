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
using System.Data.Entity;
using System.IO;

namespace proiectDaw.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        private ApplicationDbContext db = new ApplicationDbContext();

		public ActionResult Index()
        {
            String userid = User.Identity.GetUserId();
			//var profile = from profileUser in db.Profiles where profileUser.UserId == userid select profileUser;
            
            if (!db.Profiles.Any(p => p.UserId == userid))
            {
                Profile profileNew = new Profile
                {
                    Username = "",
                    LastName = "",
                    FirstName = "",
                    Age = -1,
                    UserId = userid
                };
                db.Profiles.Add(profileNew);
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            else
            {   // TODO : profile pic ca la pictures asta e la edit nu la show
                //var albums = from albm in db.Albums where albm.UserId == userid orderby albm.Name descending select albm;

                //var pics = from pic in db.Pictures where pic.UserId == userid select pic;

                var albums = db.Albums.Where(a => a.UserId == userid).Include(c => c.Pictures).ToList();
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
                //var profile = db.Profiles.Where(p => p.UserId == userid);
                ViewBag.nume = albums.ElementAt(0).Id;
                ViewBag.Albums = albums;
                ViewBag.Profiles = profile;
                /*
				ViewBag.Albums = albums;
				var pics = from pic in db.Pictures where pic.UserId == userid select pic;
				ViewBag.Pictures = pics;
				ViewBag.Profile = profile;
				*/
                return View(profile);
            }
		}

        public ActionResult Edit()
        {
            String userid = User.Identity.GetUserId();
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
            ViewBag.Profile = profile;   
            return View(profile);
        }


        [HttpPut]
        public ActionResult Edit(Profile requestProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    String userid = User.Identity.GetUserId();
                    var profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
                    if (TryUpdateModel(profile))
                    {
                        profile.Username = requestProfile.Username;
                        profile.LastName = requestProfile.LastName;
                        profile.Age = requestProfile.Age;
                        string fileName = Path.GetFileNameWithoutExtension(requestProfile.ProfileImageFile.FileName);
                        string extension = Path.GetExtension(requestProfile.ProfileImageFile.FileName);
                        fileName = fileName + "_" + DateTime.Now.ToString("yymmssfff") + "_" + extension;
                        profile.ProfileImagePath = "~/Image/" + fileName;
                        fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
                        profile.ProfileImageFile.SaveAs(fileName);
                        db.SaveChanges();
                        TempData["message"] = "Profilul a fost modificat!";
                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    return View();
                }

            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}