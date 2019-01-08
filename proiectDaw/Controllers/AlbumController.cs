using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class AlbumController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: Album
		

		public ActionResult New()
		{
			return View();
		}
		[HttpPost]
		public ActionResult New(Albums album)
		{
			try
			{   album.UserId = User.Identity.GetUserId();
				db.Albums.Add(album);  // TODO:
				db.SaveChanges();
				TempData["message"] = "Albumul a fost adaugat!";
				return RedirectToAction("Show",new { id = album.Id });
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
			// poate merge si cu aia din clasa
			var pics = from pic in db.Pictures where pic.AlbumId == id orderby pic.Date descending select pic;
			ViewBag.Pictures = pics;
			return View(album);
			//return View();
		}
		//TODO: admin only 
		public ActionResult Edit(int id)
		{
			Albums album = db.Albums.Find(id);
			ViewBag.Album = album;
			return View(album);
		}

		[HttpPut]
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
						TempData["message"] = "Albumul a fost modificata!";
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
		public ActionResult Delete(int id)
		{
			Categories category = db.Categories.Find(id);
			db.Categories.Remove(category);
			db.SaveChanges();
			TempData["message"] = "Categoria a fost stearsa!";
			return RedirectToAction("Index");
		}
	}
}