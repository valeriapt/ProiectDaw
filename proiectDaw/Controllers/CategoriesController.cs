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

		public ActionResult New()
		{
			return View();
		}
		[HttpPost]
		public ActionResult New(Categories category)
		{
			try
			{
				db.Categories.Add(category);  // TODO:
				db.SaveChanges();
				TempData["message"] = "Categoria a fost adaugata!";
				return RedirectToAction("Index");
			}
			catch (Exception e)
			{
				return View();
			}
		}

		public ActionResult Show(int id)
		{
			Categories category = db.Categories.Find(id);
			ViewBag.Category = category;

			return View(category);
		}
		//TODO: admin only 
		public ActionResult Edit(int id)
		{
			Categories category = db.Categories.Find(id);
			ViewBag.Category = category;
			return View(category);
		}

		[HttpPut]
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
						TempData["message"] = "Categoria a fost modificata!";
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