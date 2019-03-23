using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class PictureController : Controller
    {
		private ApplicationDbContext db = new ApplicationDbContext();
		[HttpGet]
		[Authorize(Roles = "Editor")]
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
			string userid = User.Identity.GetUserId();
			var albums = from albm in db.Albums where albm.UserId == userid select albm;
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
		[Authorize(Roles = "Editor")]
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
            PicModel.Date = DateTime.Now;
            string userid = User.Identity.GetUserId();
            PicModel.UserId = userid;
            db.Pictures.Add(PicModel);
            db.SaveChanges();
			ModelState.Clear();
			TempData["message"] = "The photo was added!";
			return RedirectToAction("Index", "Profile");
			
            //return View(PicModel);
		}

		
		public ActionResult Show(int id)
		{
			string userid = User.Identity.GetUserId();
			Pictures picModel = db.Pictures.Find(id);
			var comments = db.Comments.Where(a => a.PictureID == id).ToList();
			if (User.IsInRole("Administrator")) ViewBag.IsAdmin = true;
			else ViewBag.IsAdmin = false;

			if (User.IsInRole("Administrator") || picModel.UserId == userid) ViewBag.CanEdit = true;
			else ViewBag.CanEdit = false;

			ViewBag.Userid = userid;
			ViewBag.Picture = picModel;
            ViewBag.Category = picModel.Categories;
            ViewBag.Album = picModel.Albums;
			ViewBag.Comments = comments;
			if (TempData.ContainsKey("message"))
			{
				ViewBag.message = TempData["message"].ToString();
			}
            return View(picModel);
		}

		[HttpPost]
		[Authorize(Roles = "Editor, Administrator")]
		public ActionResult Edit(int id)
		{
			string userid = User.Identity.GetUserId();
			Pictures picModel = db.Pictures.Find(id);
			if (User.IsInRole("Administrator") || picModel.UserId == userid)
			{
				ViewBag.Picture = picModel;
				return View(picModel);
			}
			else
			{
				TempData["message"] = "You don't have the right to edit that photo!";
				ViewBag.message = TempData["message"].ToString();
				return RedirectToAction("Show", "Picture", new { id = picModel.Id });
			}
			
			
		}
		
		public ActionResult GetImage(int photoId, string horizontalFlip = "", string verticalFlip = "",
						string rotateLeft = "", string rotateRight = "")
		{
			var photo= db.Pictures.Find(photoId);
			var image = new WebImage(photo.ImagePath);
			var fileNameOld = photo.ImagePath;
			if (!string.IsNullOrWhiteSpace(verticalFlip))
				image = image.FlipVertical();
			if (!string.IsNullOrWhiteSpace(horizontalFlip))
				image = image.FlipHorizontal();
			if (!string.IsNullOrWhiteSpace(rotateLeft))
				image = image.RotateLeft();
			if (!string.IsNullOrWhiteSpace(rotateRight))
				image = image.RotateRight();
			string fileName = Path.GetFileNameWithoutExtension(photo.ImagePath);
			string extension = Path.GetExtension(photo.ImagePath);
			fileName = fileName + "_" + DateTime.Now.ToString("yymmssfff") + "_" + extension;
			var newImagePath = "~/Image/" + fileName;
			fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
			image.Save(newImagePath);
			newImagePath = image.FileName;
			photo.ImagePath = image.FileName;
			db.SaveChanges();
			return RedirectToAction("Show", "Picture", new { id = photoId });
		}

		[HttpDelete]
		[Authorize(Roles = "Editor, Administrator")]
		public ActionResult Delete(int id)
		{
			Pictures pic = db.Pictures.Find(id);
			string userid = User.Identity.GetUserId();
			if (userid == pic.UserId || User.IsInRole("Administrator"))
			{
				db.Pictures.Remove(pic);
				db.SaveChanges();
				TempData["message"] = "Picture deleted !";
				return RedirectToAction("Index", "Profile");
			}
			else
			{
				TempData["message"] = "You don't have the right to delete this picture!";
				return RedirectToAction("Index", "Profile");
			}
		}


	}
}