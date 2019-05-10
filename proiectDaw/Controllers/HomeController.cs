using Microsoft.AspNet.Identity;
using proiectDaw.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proiectDaw.Controllers
{

    public class HomeController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            String userid = User.Identity.GetUserId();
            if (!User.IsInRole("Editor"))
            {
                var pictures = db.Pictures.Include("Category").Include("Album").OrderByDescending(p => p.Date);//from pic in db.Pictures orderby pic.Date descending select pic;
                ViewBag.Pictures = pictures;
                return View();
            }
            else
            {

                foreach (Score i in db.Score)
                    db.Score.Remove(i);
                foreach (Sugestii i in db.Sugestii)
                    db.Sugestii.Remove(i);
                // db.sugestii.R
                db.SaveChanges();
                var vecini = GetNearestNeighbors(userid, 5).ToList();
                var nrPoze = db.Pictures.Count();
                var poze = db.Pictures.ToList();
                var recomandari = new List<Sugestii>().ToList();
                for (int i = 0; i < nrPoze; i++)
                {
                    var scorPoza = 0.0;
                    var likedPhoto = db.Likes.ToList().Where(a => (a.UserId == userid) && (a.PhotoId == poze[i].Id)).ToList();
                    if (poze[i].UserId == userid || likedPhoto.Any()) scorPoza = double.NegativeInfinity;
                    else
                    {
                        int count = 0;
                        for (int u = 0; u < 4; u++)
                        {
                            var like = db.Likes.ToList().Where(a => (a.UserId == vecini[u]) && (a.PhotoId == poze[i].Id)).ToList();
                            var nota = 0.0;
                            if (like.Any()) nota = like[0].Rating;
                            if (nota > 0)
                            {
                                scorPoza += nota;
                                count++;
                            }
                        }
                        if (count > 0) scorPoza = scorPoza / count;
                    }
                    Sugestii s = new Sugestii
                    {
                        Score = scorPoza,
                        PozaId = poze[i].Id
                    };
                    recomandari.Add(s);
                }

                recomandari = recomandari.OrderByDescending(x => x.Score).ToList();
                var pictures = new List<Pictures>();
                for (int i = 0; i <= 5; i++)
                {
                    var pic = db.Pictures.ToList().Where(a => a.Id == recomandari[i].PozaId).First();
                    pictures.Add(pic);
                }

                // var pictures = db.Pictures.Include("Category").Include("Album").OrderByDescending(p => p.Date);//from pic in db.Pictures orderby pic.Date descending select pic;

                ViewBag.Pictures = pictures;
                return View();
            }

        }


        public List<string> GetNearestNeighbors(string userid, int nrVecini)
        {
            var nrUsers = db.Users.Count();
            var usr = from u in db.Users select u;
            var users = usr.ToList();
            var scor = new List<Score>();

            for (int i = 0; i < nrUsers; i++)
                if (users[i].Id == userid)
                {
                    Score s = new Score { Scorul = double.NegativeInfinity, User = userid };
                    scor.Add(s);
                }
                else
                {
                    Score s = new Score { Scorul = GetScore(userid, users[i].Id), User = users[i].Id };
                    scor.Add(s);
                }
            var SimilarScores = scor.OrderByDescending(x => x.Scorul).ToList();
            var SimilarUsers = new List<string>();
            for (int i = 0; i < nrVecini; i++) SimilarUsers.Add(SimilarScores[i].User);
            return SimilarUsers;
        }

        public double GetScore(string user1, string user2)
        {
            //    var likesUser = db.Likes.Where(a => a.UserId == userid).ToList();
            // cousineUser algorithm

            var nrPoze = db.Pictures.Count();
            // var poze = db.Pictures.ToList();
            var p = from poz in db.Pictures select poz;
            var poze = p.ToList();
            var SumPoze = 0.0;
            var SumUser1 = 0.0;
            var SumUser2 = 0.0;
            for (int i = 0; i < nrPoze; i++)
            {
                var like1 = db.Likes.ToList().Where(a => a.UserId == user1).Where(a => a.PhotoId == poze[i].Id).ToList();
                var like2 = db.Likes.ToList().Where(a => (a.UserId == user2) && (a.PhotoId == poze[i].Id)).ToList();
                var nota1 = 0;
                var nota2 = 0;
                if (like1.Any()) nota1 = like1[0].Rating;
                if (like2.Any()) nota2 = like2[0].Rating;
                SumPoze += nota1 * nota2;
                SumUser1 += nota1;
                SumUser2 += nota2;
            }
            if (SumUser1 == 0 || SumUser2 == 0) return 0;
            return SumPoze / (Math.Sqrt(SumUser1) * Math.Sqrt(SumUser2));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}