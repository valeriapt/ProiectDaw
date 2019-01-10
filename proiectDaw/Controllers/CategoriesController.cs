using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class CategoriesController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		// GET: Categories
		[Authorize(Roles = "Administrator")]
		public ActionResult Index()
        {
			var categories = from cat in db.Categories orderby cat.Name select cat;
			ViewBag.Categories = categories;
			if (TempData.ContainsKey("message"))
			{
				ViewBag.message = TempData["message"].ToString();
			}
            return View();
        }

		[Authorize(Roles = "Administrator")]
		public ActionResult New()
		{
			return View();
		}

		[HttpPost]
		[Authorize(Roles = "Administrator")]
		public ActionResult New(Categories category)
		{
			try
			{
				db.Categories.Add(category); 
				db.SaveChanges();
				TempData["message"] = "Category added!";
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				return View();
			}
		}

		
		public ActionResult Show(int id)
		{
			if (User.IsInRole("Administrator"))
			{
				ViewBag.IsAdmin = true;
			}
			else ViewBag.IsAdmin = false;
			Categories category = db.Categories.Find(id);
			ViewBag.Category = category;
			var pics = from pic in db.Pictures where pic.CategoryId == id orderby pic.Date descending select pic;
			ViewBag.Pictures = pics;
			return View(category);
			//return View();
		}

		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id)
		{
			Categories category = db.Categories.Find(id);
			ViewBag.Category = category;
			return View(category);
		}

		[HttpPut]
		[Authorize(Roles = "Administrator")]
		public ActionResult Edit(int id, Categories requestCategory)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Categories category = db.Categories.Find(id);
					if (TryUpdateModel(category))
					{
						category.Name = requestCategory.Name;
						db.SaveChanges();
						TempData["message"] = "Category updated!";
					}
					return RedirectToAction("Index");
				}
				else
				{
					return View(requestCategory);
				}

			}
			catch (Exception e)
			{
				return View(requestCategory);
			}
		}

		[HttpDelete]
		[Authorize(Roles = "Administrator")]
		public ActionResult Delete(int id)
		{
			Categories category = db.Categories.Find(id);
			db.Categories.Remove(category);
			db.SaveChanges();
			TempData["message"] = "Category deleted!";
			return RedirectToAction("Index");
		}

	}
}