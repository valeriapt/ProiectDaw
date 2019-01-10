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
        
        //TODO: ca sa faca validarile, campurile pentru care se fac validari trebuie sa fie Required!!!
        [HttpPost]
		[Authorize(Roles = "Editor")]
		public ActionResult New(Comments comment, int? photoId)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                Profile profile = db.Profiles.SingleOrDefault(p => p.UserId == userid);
				comment.userId = userid;
				comment.CreatedBy = profile.Username;
                comment.CreatedAt = DateTime.Now;
                comment.PictureID = (int) photoId;
                db.Comments.Add(comment);
                db.SaveChanges();
                TempData["message"] = "Comment added!";
                return RedirectToAction("Show","Picture", new { id = photoId });
            }
            catch (Exception e)
            {
				//return View();
				return RedirectToAction("Show", "Picture", new { id = photoId });
			}
        }

		[Authorize(Roles = "Editor")]
		public ActionResult Edit(int id)
		{
			Comments com = db.Comments.Find(id);
			ViewBag.Comments = com;
			return View(com);
		}

		[HttpPut]
		[Authorize(Roles = "Editor")]
		public ActionResult Edit(int id, Comments requestComment)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Comments com = db.Comments.Find(id);
					if (TryUpdateModel(com))
					{
						com.Text = requestComment.Text;
						db.SaveChanges();
						TempData["message"] = "Comment updated!";
					}
					return RedirectToAction("Show", "Picture", new { id = com.PictureID});
				}
				else
				{
					return View(requestComment);
				}

			}
			catch (Exception e)
			{
				return View(requestComment);
			}
		}

		[HttpDelete]
		[Authorize (Roles = "Editor, Administrator")]
		public ActionResult Delete(int id)
		{
			Comments comment = db.Comments.Find(id);
			string userid = User.Identity.GetUserId();
			if (userid == comment.userId || User.IsInRole("Administrator"))
			{
				db.Comments.Remove(comment);
				db.SaveChanges();
				TempData["message"] = "Comment deleted !";
				return RedirectToAction("Show","Picture",new {id = comment.PictureID } );
			}
			else
			{
				TempData["message"] = "You don't have the right to delete this comment!";
				return RedirectToAction("Show", "Picture", new { id = comment.PictureID });
			}
		}


	}
}