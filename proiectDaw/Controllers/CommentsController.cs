using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class CommentsController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comments
        /*public ActionResult Index(int id)
        { 

            return View();
        }*/

        [HttpPost]
        public ActionResult New(Comments comment, int? photoId)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
                comment.CreatedBy = profile.Username;
                comment.CreatedAt = DateTime.Now;
                comment.PictureID = (int) photoId;
                db.Comments.Add(comment);
                db.SaveChanges();
                TempData["message"] = "Comentariul a fost adaugat!";
                return RedirectToAction("Show","Picture", new { id = photoId });
            }
            catch (Exception e)
            {
                return View();
            }
        }
    }
}