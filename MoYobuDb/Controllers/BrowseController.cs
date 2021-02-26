using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using Newtonsoft.Json;

namespace MoYobuDb.Controllers
{
    [AllowAnonymous]
    public class BrowseController : Controller
    {
        [HttpGet]
        public IActionResult Anime(string format, int year, string studio, int? page = null)
        {
            
            ViewData["format"] = Helper.GetAnimeFormats();
            ViewData["years"] = Helper.GetAnimeYears();
            ViewData["studios"] = Helper.GetAnimeStudios();


            BrowseFilter browseFilter = new BrowseFilter()
            {
                format = format,
                studio = studio,
                year = year.ToString()
            };
            AnimeDao animeDao = new AnimeDao(browseFilter);
            
            return View(animeDao.FindAll(page));
        }
        
        [HttpGet]
        public IActionResult Manga(string format, string status, string score, int? page = null)
        {
            ViewData["format"] = Helper.GetMangaFormats();
            ViewData["status"] = Helper.GetMangaStatus();
            ViewData["score"] = Helper.GetMangaScore();
            
            BrowseFilter browseFilter = new BrowseFilter()
            {
                format = format,
                score = score,
                status = status
            };
            
            MangaDao mangaDao = new MangaDao(browseFilter);
            
            return View(mangaDao.FindAll(page));
        }

        [HttpGet]
        public IActionResult Studios(int? page = null)
        {
            StudioDao studioDao = new StudioDao();
            return View(studioDao.FindAll(page));
        }

        [HttpGet]
        [Route("API/GET/ANIME/PAGE/")]
        public IActionResult FindJson(int? page = null)
        {
            AnimeDao animeDao = new AnimeDao();
            // foreach (var item in animeDao.FindAll(null))
            // {
            //     Debug.WriteLine(item.englishTitle);
            // }

            return Content(JsonConvert.SerializeObject(animeDao.FindAll(page)), "application/json");
        }
        
        [HttpGet]
        [Route("API/GET/MANGA/PAGE/")]
        public IActionResult FindMangaJson(int? page = null)
        {
            MangaDao mangaDao = new MangaDao();
            // foreach (var item in mangaDao.FindAll(null))
            // {
            //     Debug.WriteLine(item.englishTitle);
            // }

            return Content(JsonConvert.SerializeObject(mangaDao.FindAll(page)), "application/json");
        }

        [HttpGet]
        [Route("API/GET/ANIME/FIND")]
        public IActionResult FindAnimeByNameJson(string name)
        {
            AnimeDao animeDao = new AnimeDao();
            string temp =
                string.Format(
                    "(romaji_title COLLATE BINARY_CI like '%{0}%' OR japanese_title COLLATE BINARY_CI like '%{0}%' OR english_title COLLATE BINARY_CI like '%{0}%')",
                    name);
            return Content(JsonConvert.SerializeObject(animeDao.FindBy(temp + " AND ROWNUM < 21", null)));
        }
        
        [HttpGet]
        [Route("API/GET/STUDIO/FIND")]
        public IActionResult FindStudioByNameJson(string name)
        {
            StudioDao studioDao = new StudioDao();
            string temp =
                string.Format(
                    "(name COLLATE BINARY_CI like '%{0}%')",
                    name);
            return Content(JsonConvert.SerializeObject(studioDao.FindBy(temp + " AND ROWNUM < 21", null)));
        }
        
        [HttpGet]
        [Route("API/GET/MANGA/FIND")]
        public IActionResult FindMangaByNameJson(string name)
        {
            MangaDao mangaDao = new MangaDao();
            string temp =
                string.Format(
                    "(romaji_title COLLATE BINARY_CI like '%{0}%' OR japanese_title COLLATE BINARY_CI like '%{0}%' OR english_title COLLATE BINARY_CI like '%{0}%')",
                    name);
            return Content(JsonConvert.SerializeObject(mangaDao.FindBy(temp + " AND ROWNUM < 21", null)));
        }
    }
}