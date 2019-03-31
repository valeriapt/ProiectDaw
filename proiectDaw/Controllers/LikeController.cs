using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{
    public class LikeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Comments

        //TODO: ca sa faca validarile, campurile pentru care se fac validari trebuie sa fie Required!!!
        [HttpPost]
        [Authorize(Roles = "Editor")]
        public ActionResult New(Likes like, int? photoId)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
                like.UserId = userid;
                like.PhotoId = (int)photoId;
                db.Likes.Add(like);
                db.SaveChanges();
                TempData["message"] = "Picture liked";
                return RedirectToAction("Show", "Picture", new { id = photoId });
            }
            catch (Exception e)
            {
                //return View();
                return RedirectToAction("Show", "Picture", new { id = photoId });
            }
        }


        [HttpDelete]
        [Authorize(Roles = "Editor, Administrator")]
        public ActionResult Delete(int id)
        {
            Likes like = db.Likes.Find(id);
            string userid = User.Identity.GetUserId();
            if (userid == like.UserId || User.IsInRole("Administrator"))
            {
                db.Likes.Remove(like);
                db.SaveChanges();
                TempData["message"] = "Like deleted !";
                return RedirectToAction("Show", "Picture", new { id = like.PhotoId });
            }
            else
            {
                TempData["message"] = "You don't have the right to delete this comment!";
                return RedirectToAction("Show", "Picture", new { id = like.PhotoId });
            }
        }
    }
}