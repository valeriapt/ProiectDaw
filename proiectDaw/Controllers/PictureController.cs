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

            //new
            ViewBag.Picture = picModel;
            ViewBag.Category = picModel.Categories;
            ViewBag.Album = picModel.Albums;
            /*
			using (ApplicationDbContext db = new ApplicationDbContext())
			{
				picModel = db.Pictures.Where(x => x.Id == id).FirstOrDefault();

			}*/

            return View(picModel);
		}

		/*
        // GET: Picture
        public ActionResult Index()
        {
			
            List<Pictures> images = new List<Pictures> {
                new Pictures{Id = 1,
                            Name = "Poza 1",
                            Image = System.IO.File.ReadAllBytes(@"E:\Google\AndroidFundamentals\app\src\main\res\drawable\poster_sparkling.jpg"),
                            CategoryId = 1 },
                new Pictures{Id = 2,
                            Name = "Poza 2",
                            Image = System.IO.File.ReadAllBytes(@"E:\Google\AndroidFundamentals\app\src\main\res\drawable\poster_sparkling.jpg"),
                            CategoryId = 1 } };

            return View(images);
        }

        [HttpPost]
        public ActionResult Index(int imageId)
        {
            List<Pictures> images = new List<Pictures> {
                new Pictures{Id = 1,
                            Name = "Poza 1",
                            Image = System.IO.File.ReadAllBytes(@"E:\Google\AndroidFundamentals\app\src\main\res\drawable\poster_sparkling.jpg"),
                            CategoryId = 1 },
                new Pictures{Id = 2,
                            Name = "Poza 2",
                            Image = System.IO.File.ReadAllBytes(@"E:\Google\AndroidFundamentals\app\src\main\res\drawable\poster_sparkling.jpg"),
                            CategoryId = 1 } };
            /* Pictures image = images.Find(p => p.Id == imageId);
             if (image != null)
             {
                 ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(image.Image, 0, image.Image.Length);
             }
            //ViewBag.Base64String = "data:image/png;base64," + Convert.ToBase64String(images.Image, 0, image.Image.Length);
			
            return View(images);

        }
		*/

		/*ivate List<Pictures> GetImages()
		 {
			 string query = "SELECT * FROM 
		 }*/
	}
}