using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    public class MangaController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;

        public MangaController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        [Route("Manga/Details/{id}/{name?}")]
        public IActionResult Details(int id, string name)
        {
            StaffDao sd = new StaffDao();
            MangaDao mangaDao = new MangaDao();
            CharacterDao characterDao = new CharacterDao("MANGA_CHARACTERS");
            ReviewsDao reviewsDao = new ReviewsDao();

            Manga manga = mangaDao.FindById(id);

            manga.characters = characterDao.FindBy("ac.MANGA_ID = " + id, "MANGA_CHARACTERS");
            manga.reviews = reviewsDao.FindAllById(id);
            //manga.reviews = reviewsDao.FindBy("r.reviews_thread_id = " + id, "mangas");
            manga.staffs = sd.FindBy("a.manga_id = " + id, "MANGA_STAFFS");

            //MyListDao mld = new MyListDao();
            //var t = mld.FindAllById("' AND l.TYPE = 'manga'");

            if (mangaDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = mangaDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["mangaLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");
            }
            else
                ViewData["mangaLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");


            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "MANGA_ID");
            //ViewData["mangaLists"] = Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");

            return View(manga);
        }


        [HttpGet]
        [Route("API/GET/MANGA/{id}")]
        public IActionResult DetailsJson(int id)
        {
            MangaDao mangaDao = new MangaDao();

            return Content(JsonConvert.SerializeObject(mangaDao.FindById(id)), "application/json");
        }

        [HttpGet]
        [Route("Manga/Details/Characters/{id}/{name?}")]
        public IActionResult Characters(int id, string name)
        {
            MangaDao mangaDao = new MangaDao();
            CharacterDao characterDao = new CharacterDao("MANGA_CHARACTERS");

            Manga manga = mangaDao.FindById(id);
            manga.characters = characterDao.FindBy("ac.manga_id = " + id, "MANGA_CHARACTERS");

            if (mangaDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = mangaDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["mangaLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");
            }
            else
                ViewData["mangaLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");


            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "MANGA_ID");

            return View(manga);
        }

        [HttpGet]
        [Route("Manga/Details/Reviews/{id}/{name?}")]
        public IActionResult Reviews(int id, string name)
        {
            MangaDao mangaDao = new MangaDao();
            ReviewsDao reviewsDao = new ReviewsDao();

            Manga manga = mangaDao.FindById(id);
            manga.reviews = reviewsDao.FindBy("r.reviews_thread_id = " + id, "mangas");

            if (mangaDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = mangaDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["mangaLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");
            }
            else
                ViewData["mangaLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");

            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "MANGA_ID");
            //ViewData["mangaLists"] = Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");

            return View(manga);
        }

        [HttpGet]
        [Route("Manga/Details/Staffs/{id}/{name?}")]
        public IActionResult Staffs(int id, string name)
        {
            MangaDao mangaDao = new MangaDao();
            StaffDao sd = new StaffDao();

            Manga manga = mangaDao.FindById(id);
            manga.staffs = sd.FindBy("a.manga_id = " + id, "MANGA_STAFFS");

            if (mangaDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = mangaDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["mangaLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");
            }
            else
                ViewData["mangaLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");


            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "MANGA_ID");
            //ViewData["mangaLists"] = Helper.GetUserLists(null,User.FindFirstValue(ClaimTypes.NameIdentifier), "manga");

            return View(manga);
        }

        //[Route("Anime/Details/{id}/{name?}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToList(int id, string name, string listId, [Bind("listId")] MangaList mangaListData)
        {
            //TODO: Přidat validaci, že pokud už anime je v listu a uživatel přidá do listu, udpatuje se jeho pozice
            MangaListDao mld = new MangaListDao();
            MangaDao mangaDao = new MangaDao();
            
            mangaListData.mangaId = id;
            mangaListData.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (listId != 0.ToString())
            {
                //if (!mld.IsExist(id, mangaListData.listId))
                if (!mangaDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                    mld.Save(mangaListData);
                else
                    mld.Edit(mangaListData);
            }

            return RedirectToAction("Details", new {id, name});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToFavourite(int id, string name, string akce, [Bind("mangaId")] Manga manga)
        {
            FavouriteDao fd = new FavouriteDao("MANGA_ID");
            Favourite f = new Favourite()
            {
                animeId = null,
                mangaId = manga.mangaId,
                type = "manga",
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            if (akce == "images/heart0.svg")
            {
                if (!fd.IsExist(id, f.userId, "MANGA_ID"))
                    fd.Save(f);
            }
            else if (akce == "images/heart1.svg")
                fd.Delete(f);

            return RedirectToAction("Details", new {id, name});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(
            [Bind(
                "englishTitle,japaneseTitle,romajiTitle,mangaImg,format,chapters,anilistId,malId,score,source,startDate,endDate,status")]
            Manga manga)
        {
            var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\mangaImg"),
                "mangaImg", HttpContext.Request.Form.Files);
            manga.img = temp;

            MangaDao mangaDao = new MangaDao();
            try
            {
                mangaDao.Save(manga);
            }
            catch (OracleException e)
            {
                return View(manga);
            }

            return RedirectToAction("Index", "Administration");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, string name)
        {
            MangaDao mangaDao = new MangaDao();

            Manga manga;
            try
            {
                manga = mangaDao.FindById(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return View(manga);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(
            [Bind(
                "mangaId,englishTitle,japaneseTitle,romajiTitle,descriptions,mangaImg,format,chapters,anilistId,malId,score,source,startDate,endDate,status,reviewsThreadId")]
            Manga manga)
        {
            if (!HttpContext.Request.Form.Files.IsNullOrEmpty())
            {
                var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\mangaImg"),
                    "mangaImg", HttpContext.Request.Form.Files);
                manga.img = temp;
            }

            MangaDao mangaDao = new MangaDao();

            mangaDao.Edit(manga);
            return RedirectToAction("Index", "Administration");
            //}

            return View(manga);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            //TODO: Pořesit bez id -> int musí byt nullable
            // if (id == null)
            // {
            //     return NotFound();
            // }

            MangaDao mangaDao = new MangaDao();

            return View(mangaDao.FindById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, Manga mangaData)
        {
            MangaDao mangaDao = new MangaDao();
            mangaDao.Delete(mangaData);

            return RedirectToAction("Index", "Administration");
        }
    }
}