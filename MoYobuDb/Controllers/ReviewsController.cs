using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoYobuDb.Data;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Reviews
        public IActionResult Index()
        {
            return View();
        }

        // GET: Reviews/Details/5
        public IActionResult Details(int id)
        {
            ReviewsDao reviewsDao = new ReviewsDao();
            Review review = reviewsDao.FindById(id);
            return View(review);
        }

        // GET: Reviews/Create
        [Authorize(Roles = "Admin,Reviewer")]
        [Route("Reviews/Create/{table}/{id}")]
        public IActionResult Create(int id, string table, string nameId)
        {
            ReviewsDao rd = new ReviewsDao();
            ReviewsThreadDao rtd = new ReviewsThreadDao();
            rd.rev_id = id;
            rtd.rev_id = id;
            string thread = rd.FindThread(table, nameId, id);
            Debug.WriteLine("thread : " + thread);

            Debug.WriteLine(thread);
            if (rd.FindThreadBool("REVIEWS_THREADS", id))
            {
                Debug.WriteLine("prošlo");
                // Zjistí thread a přidá to k danému thread
            }
            else
            {
                Debug.WriteLine("neprošlo");
                // Vytvoří thread, přidá ho k anime a recenze se pak přidá do daného thread
                rtd.Save(null);

                if (table == "animes")
                {
                    AnimeDao animeDao = new AnimeDao();
                    Anime a = new Anime() {animeId = id, reviewsThreadId = id};
                    animeDao.EditThread(id);
                }
                else if (table == "mangas")
                {
                    MangaDao mangaDao = new MangaDao();
                    Manga m = new Manga() {mangaId = id, reviewsThreadId = id};
                    mangaDao.EditThread(id);
                }
            }

            ViewData["reviewsThreadId"] = id;
            return View();
        }


        [HttpPost]
        [Authorize(Roles = "Admin,Reviewer")]
        [Route("Reviews/Create/{table}/{id}")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(int id, string table, [Bind("content,score,title,myDate")] Review review)
        {
            if (ModelState.IsValid)
            {
                Review r = new Review
                {
                    userId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    content = review.content.Replace("'", "''"),
                    score = review.score,
                    title = review.title,
                    reviewsThreadId = id
                };
                ReviewsDao reviewsDao = new ReviewsDao();
                try
                {
                    reviewsDao.Save(r);
                }
                catch (OracleException e)
                {
                    Debug.WriteLine("Chyba uložení do databáze\n{0}", e);
                    return View(review);
                }
            }

            if (table == "animes")
                return RedirectToAction("Details", "Animes", id);
            return RedirectToAction("Details", "Manga", id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Reviews/Edit/5
        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public IActionResult Edit(int id)
        {
            ReviewsDao reviewsDao = new ReviewsDao();
            return View(reviewsDao.FindById(id));
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Reviewer")]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("content,score,title")] Review review)
        {
            // if (id != review.reviewId)
            // {
            //     return NotFound();
            // }

            if (ModelState.IsValid)
            {
                ReviewsDao reviewsDao = new ReviewsDao();
                review.reviewId = id;
                reviewsDao.Edit(review);
                return RedirectToAction("Reviews", "User");
            }

            //return RedirectToAction(nameof(Index));
            return View(review);
        }

        // GET: Reviews/Delete/5
        [HttpGet]
        [Authorize(Roles = "Admin,Reviewer")]
        public IActionResult Delete(int id)
        {
            ReviewsDao reviewsDao = new ReviewsDao();

            return View(reviewsDao.FindById(id));
        }

        // POST: Reviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Reviewer")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Review reviewData)
        {
            ReviewsDao reviewsDao = new ReviewsDao();
            reviewsDao.Delete(reviewData);

            return RedirectToAction("Reviews", "User");
        }

        private bool ReviewExists(int id)
        {
            return _context.reviews.Any(e => e.reviewId == id);
        }
    }
}