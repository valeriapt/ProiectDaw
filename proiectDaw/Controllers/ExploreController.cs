using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class ExploreController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		[HttpGet]
		// GET: Explore
		public ActionResult Index(string Text)
        {
			var pictures = db.Pictures.Where(m => (m.Name.Contains(Text) || m.Description.Contains(Text))).ToList();
			//from pic in db.Pictures where (pic.Name.Contains(Text) || pic.Description.Contains(Text)) select pic;
			var categories = db.Categories.ToList();
			ViewBag.Categories = categories;
			if (Text == "" || Text == null)
			{	
				return View();
			}
			else {
				if (db.Pictures.Any(m => (m.Name.Contains(Text) || m.Description.Contains(Text))))
				{
					TempData["message"] = "Exista rezultate";
					ViewBag.message = TempData["message"].ToString();

					ViewBag.Pictures = pictures;
					return View(pictures);
				}
				else
				{
					
					TempData["message"] = "Nu exista rezultate";
					ViewBag.message = TempData["message"].ToString();
					return View();
				}
			}

			
			
        }
    }
}