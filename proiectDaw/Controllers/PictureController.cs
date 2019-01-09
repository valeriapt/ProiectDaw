using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class PictureController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		[HttpGet]
		public ActionResult Add()
		{
            Pictures picture = new Pictures();
            // preluam lista de categorii din metoda GetAllCategories()
            picture.Categories = GetAllCategories();
            picture.Albums = GetAllAlbums();
            return View(picture);
		}

        [NonAction]
        public IEnumerable<SelectListItem> GetAllCategories()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var categories = from cat in db.Categories select cat;
            // iteram prin categorii
            foreach (var category in categories)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = category.Id.ToString(),
                    Text = category.Name.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }

        [NonAction]
        public IEnumerable<SelectListItem> GetAllAlbums()
        {
            // generam o lista goala
            var selectList = new List<SelectListItem>();
            // Extragem toate categoriile din baza de date
            var albums = from albm in db.Albums select albm;
            // iteram prin categorii
            foreach (var album in albums)
            {
                // Adaugam in lista elementele necesare pentru dropdown
                selectList.Add(new SelectListItem
                {
                    Value = album.Id.ToString(),
                    Text = album.Name.ToString()
                });
            }
            // returnam lista de categorii
            return selectList;
        }
		[NonAction]
		public IEnumerable<SelectListItem> GetAllComments()
		{
			// generam o lista goala
			var selectList = new List<SelectListItem>();
			// Extragem toate comentariile din baza de date
			var comments = from com in db.Comments select com;
			// iteram prin categorii
			foreach (var com in comments)
			{
				// Adaugam in lista elementele necesare pentru dropdown
				selectList.Add(new SelectListItem
				{
					Value = com.Id.ToString(),
					Text = com.Text.ToString()
				});
			}
			// returnam lista de comentariile
			return selectList;
		}



		[HttpPost]
		public ActionResult Add(Pictures PicModel)
		{
			string fileName = Path.GetFileNameWithoutExtension(PicModel.ImageFile.FileName);
			string extension = Path.GetExtension(PicModel.ImageFile.FileName);
			fileName = fileName +"_" +DateTime.Now.ToString("yymmssfff") + "_" + extension;
			PicModel.ImagePath = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"),fileName);
			PicModel.ImageFile.SaveAs(fileName);
            PicModel.Categories = GetAllCategories();
            PicModel.Albums = GetAllAlbums();
            string userid = User.Identity.GetUserId();
            PicModel.UserId = userid;
            using (ApplicationDbContext db = new ApplicationDbContext())
                    {
                        db.Pictures.Add(PicModel);
                        db.SaveChanges();
                    }
                    ModelState.Clear();
            return View(PicModel);
		}

		[HttpGet]
		public ActionResult Show(int id)
		{
			Pictures picModel = db.Pictures.Find(id);
			var comments = db.Comments.Where(a => a.PictureID == id).ToList();
            ViewBag.Picture = picModel;
            ViewBag.Category = picModel.Categories;
            ViewBag.Album = picModel.Albums;
			ViewBag.Comments = comments;
            return View(picModel);
		}

		
	}
}