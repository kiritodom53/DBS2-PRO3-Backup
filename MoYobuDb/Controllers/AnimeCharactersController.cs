using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MoYobuDb.Models.Database.Dao;

namespace MoYobuDb.Controllers
{
    public class AnimeCharactersController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            AnimeCharacterDao animeCharacter = new AnimeCharacterDao();
            foreach (var item in animeCharacter.FindAll())
            {
                Debug.WriteLine("animeCharactersId : {0}", item.animeCharactersId);
                Debug.WriteLine("animeId : {0}", item.animeId);
                Debug.WriteLine("characterId : {0}", item.characterId);
            }

            return View(animeCharacter.FindAll());
        }

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }
    }
}