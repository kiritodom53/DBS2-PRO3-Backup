using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;

namespace MoYobuDb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            AnimeDao animeDao = new AnimeDao();
            MangaDao mangaDao = new MangaDao();
            List<Anime> lastAnimes = animeDao.FindLast();
            List<Manga> lastMangas = mangaDao.FindLast();

            LastMediaModel lastMediaModel = new LastMediaModel()
            {
                lastAnimes = lastAnimes,
                lastMangas = lastMangas
            };
            return View(lastMediaModel);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}