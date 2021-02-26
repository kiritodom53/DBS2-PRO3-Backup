using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    public class CharactersController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;

        public CharactersController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            CharacterDao characterDao = new CharacterDao();
            foreach (var item in characterDao.FindAll())
            {
                Console.WriteLine(item.name);
            }

            return View();
            return View(characterDao.FindAll());
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            CharacterDao characterDao = new CharacterDao();

            return View(characterDao.FindById(id));
        }

        [HttpGet]
        [Route("API/GET/CHARACTERS/{id}")]
        public IActionResult DetailsJson(int id)
        {
            CharacterDao characterDao = new CharacterDao();
            
            return Content(JsonConvert.SerializeObject(characterDao.FindById(id)), "application/json");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("characterImg,name,nameNative,surname,surnameNative,description")]
            Character character)
        {
            var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\characterImg"),
                "characterImg", HttpContext.Request.Form.Files);
            character.mediumImgUrl = temp;

            CharacterDao characterDao = new CharacterDao();
            try
            {
                characterDao.Save(character);
            }
            catch (OracleException e)
            {
                return View(character);
            }

            return RedirectToAction("Index", "Administration");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, string name)
        {
            CharacterDao characterDao = new CharacterDao();

            Character character;

            try
            {
                character = characterDao.FindById(id);
            }
            catch (Exception)
            {
                return NotFound();
            }
            return View(character);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id,
            [Bind(
                "characterId,characterImg,name,nameNative,surname,surnameNative,description,voiceActorId")]
            Character character)
        {
            var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\characterImg"),
                "characterImg", HttpContext.Request.Form.Files);
            character.mediumImgUrl = temp;

            CharacterDao characterDao = new CharacterDao();

            characterDao.Edit(character);
            return RedirectToAction("Index", "Administration");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            CharacterDao characterDao = new CharacterDao();

            return View(characterDao.FindById(id));
        }
        
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id, Character character)
        {
            CharacterDao characterDao = new CharacterDao();
            characterDao.Delete(character);

            return RedirectToAction("Index", "Administration");
        }
    }
}