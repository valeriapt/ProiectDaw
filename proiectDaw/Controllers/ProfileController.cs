using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
		[Authorize(Roles = "Editor,Administrator")] 
		public ActionResult Index()
        {
            String userid = User.Identity.GetUserId();
			//var profile = from profileUser in db.Profiles where profileUser.UserId == userid select profileUser;
			if (TempData.ContainsKey("message"))
			{
				ViewBag.message = TempData["message"].ToString();
			}
			if (!db.Profiles.Any(p => p.UserId == userid))
            {
                Profile profileNew = new Profile
                {
                    Username = "user",
                    LastName = "LastName",
                    FirstName = "FirstName",
					Age = 1,
                    UserId = userid
                };
                db.Profiles.Add(profileNew);
                db.SaveChanges();
                return RedirectToAction("Edit");
            }
            else
            {   
                
                var albums = db.Albums.Where(a => a.UserId == userid).Include(c => c.Pictures).ToList();
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);

				if (User.IsInRole("Administrator"))
				{
					ViewBag.IsAdmin = true;
				}
				else ViewBag.IsAdmin = false;

				ViewBag.Albums = albums;
                ViewBag.Profiles = profile;
                
                return View(profile);
            }
		}

		[Authorize(Roles = "Editor,Administrator")]
		public ActionResult Edit()
        {
            String userid = User.Identity.GetUserId();
            Profile profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
            ViewBag.Profile = profile;   
            return View(profile);
        }


        [HttpPut]
		[Authorize(Roles = "Editor,Administrator")]
		public ActionResult Edit(Profile requestProfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    String userid = User.Identity.GetUserId();
                    var profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
					if (profile.Username == "user")
					{
						Albums firstAlbum = new Albums
						{
							Name = "Pictures",
							UserId = userid,
							CreatedBy = requestProfile.Username
						};
						db.Albums.Add(firstAlbum);
					}
					if (TryUpdateModel(profile))
                    {
                        profile.Username = requestProfile.Username;
                        profile.LastName = requestProfile.LastName;
                        profile.Age = requestProfile.Age;
						if (requestProfile.ProfileImageFile != null)
						{

							string fileName = Path.GetFileNameWithoutExtension(requestProfile.ProfileImageFile.FileName);
							string extension = Path.GetExtension(requestProfile.ProfileImageFile.FileName);
							fileName = fileName + "_" + DateTime.Now.ToString("yymmssfff") + "_" + extension;
							profile.ProfileImagePath = "~/Image/" + fileName;
							fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
							profile.ProfileImageFile.SaveAs(fileName);
						}
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