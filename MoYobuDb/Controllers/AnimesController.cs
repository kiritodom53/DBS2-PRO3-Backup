using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoYobuDb.Data;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using MoYobuDb.Models.Tables.Charts;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    public class AnimesController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;

        public AnimesController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Anime/Create")]
        public IActionResult Create()
        {
            StudioDao studioDao = new StudioDao();
            List<Studio> studios = studioDao.FindAll(null, 0);

            List<SelectListItem> studioSelectedList = new List<SelectListItem>();
            studioSelectedList.Add(new SelectListItem {Value = "0", Text = "0_Nemá studio", Selected = false});
            foreach (var item in studios)
            {
                Debug.WriteLine(item.name);
                studioSelectedList.Add(new SelectListItem
                {
                    Value = item.studioId.ToString(), Text = item.name, Selected = false
                });
            }

            ViewData["studioId"] = studioSelectedList;
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Anime/Create")]
        public async Task<IActionResult> Create(
            [Bind(
                "romajiTitle,japaneseTitle,englishTitle,description,animeImg,format,episodes,episodeDuration,status,startDate,endDate,season,averageScore,source,hashtag,anilistId,malId,studioId")]
            Anime animeData)
        {
            var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\animeImg"),
                "animeImg", HttpContext.Request.Form.Files);
            animeData.coverImage = temp;

            AnimeDao animeDao = new AnimeDao();
            try
            {
                animeDao.Save(animeData);
            }
            catch (OracleException e)
            {
                Debug.WriteLine("Chyba uložení do databáze\n{0}", e);
                return View(animeData);
            }

            return RedirectToAction("Details", "Animes", new {id = animeData.animeId, name = animeData.romajiTitle});
            //return RedirectToAction("Index", "Administration");
        }


        [HttpGet]
        [Route("Anime/Details/{id}/{name?}")]
        public IActionResult Details(int id, string name)
        {
            StaffDao sd = new StaffDao();
            AnimeDao animedDao = new AnimeDao();
            ReviewsDao reviewsDao = new ReviewsDao();
            Anime anime;
            try
            {
                anime = animedDao.FindById(id);
            }
            catch (Exception e)
            {
                return NotFound();
            }

            Debug.WriteLine("ac.anime_id = " + id + " and rownum <= 10");
            anime.reviews = reviewsDao.FindBy("r.reviews_thread_id = " + id, "animes");
            anime.staffs = sd.FindBy("a.anime_id = " + id, "ANIME_STAFFS");

            Debug.WriteLine("rrev");
            Debug.WriteLine(anime.reviews.Count);


            if (animedDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = animedDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["animeLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");
            }
            else
                ViewData["animeLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");


            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "ANIME_ID");

            return View(anime);
        }

        [HttpGet]
        [Route("API/GET/ANIME/{id}")]
        public IActionResult DetailsJson(int id)
        {
            AnimeDao animedDao = new AnimeDao();

            return Content(JsonConvert.SerializeObject(animedDao.FindById(id)), "application/json");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Anime/Details")]
        public IActionResult Details(int id, string name, [Bind("characterId,animeId")] AnimeCharacter animeData)
        {
            if (ModelState.IsValid)
            {
                AnimeCharacter animeCharacter = new AnimeCharacter {animeId = id, characterId = animeData.characterId};
                AnimeCharacterDao animeCharacterDao = new AnimeCharacterDao();

                animeCharacterDao.Save(animeCharacter);
                return RedirectToAction("Details", new {id, name});
            }

            return RedirectToAction("Anime", "Browse");
        }

        [HttpGet]
        [Route("Anime/Details/Characters/{id}/{name?}")]
        public IActionResult Characters(int id, string name)
        {
            AnimeDao animeDao = new AnimeDao();
            Anime anime = animeDao.FindById(id);
            Debug.WriteLine("ac.anime_id = " + id + " and rownum <= 10");

            if (animeDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = animeDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["animeLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");
            }
            else
                ViewData["animeLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");


            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "ANIME_ID");
            return View(anime);
        }

        [HttpGet]
        [Route("Anime/Details/Reviews/{id}/{name?}")]
        public IActionResult Reviews(int id, string name)
        {
            AnimeDao animeDao = new AnimeDao();
            ReviewsDao reviewsDao = new ReviewsDao();

            Anime anime = animeDao.FindById(id);
            anime.reviews = reviewsDao.FindBy("r.reviews_thread_id = " + id, "animes");

            if (animeDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = animeDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["animeLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");
            }
            else
                ViewData["animeLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");


            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "ANIME_ID");

            return View(anime);
        }

        [HttpGet]
        [Route("Anime/Details/Staffs/{id}/{name?}")]
        public IActionResult Staffs(int id, string name)
        {
            AnimeDao animeDao = new AnimeDao();
            StaffDao sd = new StaffDao();

            Anime anime = animeDao.FindById(id);
            anime.staffs = sd.FindBy("a.anime_id = " + id, "ANIME_STAFFS");

            if (animeDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = animeDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["animeLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");
            }
            else
                ViewData["animeLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");


            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "ANIME_ID");

            return View(anime);
        }

        [HttpGet]
        [Route("Anime/Details/Stats/{id}/{name?}")]
        public IActionResult Stats(int id, string name)
        {
            AnimeDao animeDao = new AnimeDao();
            if (animeDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
            {
                string listName = animeDao.GetListName(id, User.FindFirstValue(ClaimTypes.NameIdentifier));
                ViewData["animeLists"] =
                    Helper.GetUserLists(listName, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");
            }
            else
                ViewData["animeLists"] =
                    Helper.GetUserLists(null, User.FindFirstValue(ClaimTypes.NameIdentifier), "anime");

            //TODO: Dodělat statistiky
            ViewData["fav"] = Helper.GetFavInfo(id, User.FindFirstValue(ClaimTypes.NameIdentifier), "ANIME_ID");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToList(int id, string name, string listId, [Bind("listId")] AnimeList animeListData)
        {
            AnimeListDao ald = new AnimeListDao();
            AnimeDao animeDao = new AnimeDao();
            
            animeListData.animeId = id;
            animeListData.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (listId != 0.ToString())
            {
                if (!animeDao.isInList(User.FindFirstValue(ClaimTypes.NameIdentifier), id))
                    ald.Save(animeListData);
                else
                    ald.Edit(animeListData);
            }

            return RedirectToAction("Details", new {id, name});
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult AddToFavourite(int id, string name, string akce, [Bind("animeId")] Anime anime)
        {
            FavouriteDao fd = new FavouriteDao("ANIME_ID");
            Favourite f = new Favourite()
            {
                animeId = anime.animeId,
                mangaId = null,
                type = "anime",
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };
            if (akce == "images/heart0.svg")
            {
                if (!fd.IsExist(id, f.userId, "ANIME_ID"))
                    fd.Save(f);
            }
            else if (akce == "images/heart1.svg")
                fd.Delete(f);

            return RedirectToAction("Details", new {id, name});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Anime/Edit/{id}/{name?}")]
        public IActionResult Edit(int id, string name)
        {
            AnimeDao animedDao = new AnimeDao();

            Anime anime;
            try
            {
                anime = animedDao.FindById(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            List<SelectListItem> studioSelectedList = new List<SelectListItem>();
            StudioDao studioDao = new StudioDao();
            List<Studio> studios = studioDao.FindAll(null, 0);
            studioSelectedList.Add(new SelectListItem {Value = "0", Text = "Nemá studio"});
            foreach (var item in studios)
            {
                studioSelectedList.Add(new SelectListItem {Value = item.studioId.ToString(), Text = item.name});
            }

            ViewData["studioId"] = studioSelectedList;

            return View(anime);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Anime/Edit/{id}/{name?}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, string name,
            [Bind(
                "animeId,romajiTitle,japaneseTitle,englishTitle,description,animeImg,format,episodes,episodeDuration,status,startDate,endDate,season,averageScore,source,hashtag,anilistId,malId,studioId")]
            Anime data)
        {

            
            if (!HttpContext.Request.Form.Files.IsNullOrEmpty())
            {
                var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\animeImg"),
                    "animeImg", HttpContext.Request.Form.Files);
                data.coverImage = temp;
            }

            AnimeDao animeDao = new AnimeDao();

            animeDao.Edit(data);
            return RedirectToAction("Details", "Animes", new {id = data.animeId, name = data.romajiTitle});
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            AnimeDao animeDao = new AnimeDao();

            return View(animeDao.FindById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id, Anime animeData)
        {
            Debug.WriteLine("dlt-id");
            Debug.WriteLine(id);
            Debug.WriteLine("dlt-id2");
            Debug.WriteLine(animeData.animeId);
            AnimeDao animeDao = new AnimeDao();
            animeDao.Delete(animeData);

            return RedirectToAction("Index", "Administration");
        }
    }
}