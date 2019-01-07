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
			return View();
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
			using (ApplicationDbContext db = new ApplicationDbContext())
			{
				db.Pictures.Add(PicModel);
				db.SaveChanges();
			}
			ModelState.Clear();
				return View();

		}

		[HttpGet]
		public ActionResult Show(int id)
		{
			Pictures picModel = db.Pictures.Find(id);
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