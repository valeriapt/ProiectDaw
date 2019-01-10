using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class AlbumController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: Album

		[Authorize(Roles = "Editor")]
		public ActionResult New()
		{
			return View();
		}
		
		[HttpPost]
		[Authorize(Roles = "Editor")]
		public ActionResult New(Albums album)
		{
			try
			{
                string userid = User.Identity.GetUserId();
                album.UserId = userid;
                Profile user = db.Profiles.Where(u => u.UserId == userid).First();
                album.CreatedBy = user.Username;
				db.Albums.Add(album);  
				db.SaveChanges();
				TempData["message"] = "Album added!";
				return RedirectToAction("Index","Profile");
			}
			catch (Exception e)
			{
				return View();
			}
		}

		
		public ActionResult Show(int id)
		{
			Albums album = db.Albums.Find(id);
			ViewBag.Album = album;
            var pics = db.Pictures.Where(a => a.AlbumId == id).OrderByDescending(p => p.Date).Include("Album").Include("Category");
            ViewBag.Pictures = pics;
			return View(album);
		}

		[Authorize(Roles = "Editor")]
		public ActionResult Edit(int id)
		{
			Albums album = db.Albums.Find(id);
			ViewBag.Album = album;
			return View(album);
		}

		[HttpPut]
		[Authorize(Roles = "Editor")]
		public ActionResult Edit(int id, Albums requestAlbum)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Albums album = db.Albums.Find(id);
					if (TryUpdateModel(album))
					{
						album.Name = requestAlbum.Name;
						db.SaveChanges();
						TempData["message"] = "Album updated!";
					}
					return RedirectToAction("Show", new { id = album.Id });
				}
				else
				{
					return View(requestAlbum);
				}

			}
			catch (Exception e)
			{
				return View(requestAlbum);
			}
		}

		[HttpDelete]
		[Authorize(Roles = "Editor,Administrator")]
		public ActionResult Delete(int id)
		{
			Albums album = db.Albums.Find(id);
			db.Albums.Remove(album);
			db.SaveChanges();
			TempData["message"] = "The album has been deleted!";

			return RedirectToAction("Index", "Profile");
		}
	}
}