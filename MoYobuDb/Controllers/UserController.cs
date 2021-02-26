using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using MoYobuDb.Models.Tables.Charts;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    [Authorize]
    public class UserController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHostingEnvironment _appEnvironment;

        public UserController(IHostingEnvironment appEnvironment, RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _appEnvironment = appEnvironment;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("User/profile/{userName?}")]
        public IActionResult Index(string username)
        {
            UserDao userDao = new UserDao();
            AnimeDao animeDao = new AnimeDao();
            MangaDao mangaDao = new MangaDao();
            if (username == null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Debug.WriteLine(userId);
                User user = userDao.FindById(userId);
                user.favouritesAnime = animeDao.FindBy("f.user_id = '" + userId + "'", "favourite");
                user.favouritesManga = mangaDao.FindBy("f.user_id = '" + userId + "'", "favourite");
                return View(user);
            }
            else
            {
                User user;
                try
                {
                    user = userDao.FindByName(username);
                }
                catch (Exception)
                {
                    return NotFound();
                }

                user.favouritesAnime = animeDao.FindBy("f.user_id = '" + user.userAspId + "'", "favourite");
                user.favouritesManga = mangaDao.FindBy("f.user_id = '" + user.userAspId + "'", "favourite");
                return View(user);
            }
        }

        //TODO: Předělat do jiného controlleru
        [HttpPost]
        [ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int animeId, string type, string userName)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (type == "anime")
            {
                AnimeListDao animeListDao = new AnimeListDao();
                animeListDao.Delete(new AnimeList {animeId = animeId, userId = userId});
                return RedirectToAction("AnimeList", "User", new {userName = userName});
            }

            if (type == "manga")
            {
                MangaListDao mangaListDao = new MangaListDao();
                mangaListDao.Delete(new MangaList {mangaId = animeId, userId = userId});
                return RedirectToAction("MangaList", "User", new {userName = userName});
            }

            return RedirectToAction("AnimeList", "User", new {userName = userName});
        }

        [HttpGet]
        [Route("User/{userName}/AnimeList/{listName}")]
        public IActionResult AnimeList(string userName, string listName = "All")
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            MyListDao myListDao = new MyListDao(listName, "anime", id, userName);
            AnimeList al = new AnimeList();
            Debug.WriteLine("listname : " + listName);
            List<MyList> myLists;

            try
            {
                myLists = myListDao.FindBy("TYPE = 'anime'", null);
            }
            catch (Exception)
            {
                return NotFound();
            }

            ViewData["userAspId"] = userName;

            ViewData["animeLists"] = Helper.GetLists(listName, "anime");

            if (listName != "All")
            {
                foreach (var item in myLists)
                {
                    if (item.name == listName)
                    {
                        //ml.Add(item);
                        return View(new List<MyList> {item});
                    }
                }
            }

            return View(myLists);
        }

        [HttpGet]
        [Route("User/{userName}/MangaList/{listName?}")]
        public IActionResult MangaList(string userName, string listName = "All")
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            MyListDao myListDao = new MyListDao(listName, "manga", id, userName);
            Debug.WriteLine("listname : " + listName);
            List<MyList> myLists;
            ViewData["userAspId"] = userName;

            ViewData["mangaLists"] = Helper.GetLists(listName, "manga");

            try
            {
                myLists = myListDao.FindBy("TYPE = 'manga'", null);
            }
            catch (Exception)
            {
                return NotFound();
            }


            if (listName != "All")
            {
                foreach (var item in myLists)
                {
                    if (item.name == listName)
                    {
                        return View(new List<MyList> {item});
                    }
                }
            }

            return View(myLists);
        }

        [HttpGet]
        //[Route("Create/New/AnimeList")]
        public IActionResult CreateList()
        {
            var model = new MyList();
            return View(model);
        }

        [HttpPost]
        //[Route("Create/New/AnimeList")]
        [ValidateAntiForgeryToken]
        public IActionResult CreateList([Bind("listId,name,userId,type")] MyList listData)
        {
            Debug.WriteLine("Create Anime List POST");
            Debug.WriteLine("IsValid");
            MyList myList = new MyList()
            {
                name = listData.name, userId = User.FindFirstValue(ClaimTypes.NameIdentifier), type = listData.type
            };
            MyListDao myListDao = new MyListDao();
            try
            {
                myListDao.Save(myList);
            }
            catch (OracleException e)
            {
                Debug.WriteLine("Chyba uložení do databáze\n{0}", e);
                return View(listData);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditProfile()
        {
            
            Debug.WriteLine("hah0");
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Debug.WriteLine("hah");
            Debug.WriteLine(userId);
            Debug.WriteLine(userId);
            UserDao userDao = new UserDao();
            return View(userDao.FindById(userId));
            
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult EditProfile(int id, [Bind("userId,desciption,userAspId")] User userData)
        {

            UserDao userDao = new UserDao();
            userDao.Edit(userData);

            return RedirectToAction(nameof(Index));
           
        }

        [HttpGet]
        [Authorize]
        public IActionResult EditProfileImg()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Debug.WriteLine(userId);
            UserDao userDao = new UserDao();
            return View(userDao.FindById(userId));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> EditProfileImg(User user)
        {
            var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\profileImg"),
                "profileImg", HttpContext.Request.Form.Files);
            user.profileImg = temp;
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserDao userDao = new UserDao();
            user.userAspId = userId;
            userDao.EditImg(user);
            return RedirectToAction("Index");
        }
        
        [Route("User/{userName}/Reviews")]
        public IActionResult Reviews(string userName)
        {
            UserDao userDao = new UserDao();
            User user;
            try
            {
                user = userDao.FindByName(userName);
            }
            catch (Exception)
            {
                return NotFound();
            }
            
            ReviewsDao reviewsDao = new ReviewsDao();
            user.reviews = reviewsDao.FindBy("r.user_id = '" + user.userAspId + "'", null);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AskForRights(int userId)
        {
;
            UserDao userDao = new UserDao();
            userDao.ReviewRights("ASK_FOR_REVIEWER", userId);
            return RedirectToAction("Index", "User");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveRights(int userId)
        {
            UserDao userDao = new UserDao();
            userDao.ReviewRights("IS_REVIEWER", userId);

            User u = userDao.FindById(userId);

            UserRoleViewModel urvm = new UserRoleViewModel();
            urvm.IsSelected = true;
            urvm.UserName = u.username;
            urvm.UserId = u.userAspId;

            var user = await userManager.FindByIdAsync(urvm.UserId);

            IdentityResult result = null;

            if (urvm.IsSelected && !(await userManager.IsInRoleAsync(user, "Reviewer")))
            {
                result = await userManager.AddToRoleAsync(user, "Reviewer");
            }

            if (result.Succeeded)
            {
                return RedirectToAction("ReviewRightsRequest", "Administration");
            }

            return RedirectToAction("ReviewRightsRequest", "Administration");
        }

        [HttpGet]
        [Route("User/{userName}/Stats")]
        public IActionResult Stats(string userName)
        {
            StatsDao statsDao = new StatsDao();
            UserDao userDao = new UserDao();

            User user;
            
            try
            {
                user = userDao.FindByName(userName);
            }
            catch (Exception)
            {
                return NotFound();
            }

            UserStats userStats = new UserStats();
            userStats.userAnimeStats = statsDao.GetAnimeUserStats(user.userAspId);
            userStats.userMangaStats = statsDao.GetMangaUserStats(user.userAspId);
            //TODO: Pořešit, když uživatel nemá žádný naime v listu a nemá statistiky


            return View(userStats);
        }

        [HttpGet]
        [Route("API/GET/CHARTS/USER/{userName}/ANIME")]
        public IActionResult StatsAnimeJson(string userName)
        {
            StatsDao statsDao = new StatsDao();
            UserDao userDao = new UserDao();
            //return View(statsDao.GetAnimeUserStats(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Content(
                JsonConvert.SerializeObject(statsDao.GetAnimeUserStats(userDao.FindByName(userName).userAspId)),
                "application/json");
        }

        [HttpGet]
        [Route("API/GET/CHARTS/USER/{userName}/MANGA")]
        public IActionResult StatsMangaJson(string userName)
        {
            StatsDao statsDao = new StatsDao();
            UserDao userDao = new UserDao();
            //return View(statsDao.GetAnimeUserStats(User.FindFirstValue(ClaimTypes.NameIdentifier)));
            return Content(
                JsonConvert.SerializeObject(statsDao.GetMangaUserStats(userDao.FindByName(userName).userAspId)),
                "application/json");
        }
    }
}