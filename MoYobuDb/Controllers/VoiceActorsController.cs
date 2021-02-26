using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    public class VoiceActorsController : Controller
    {
        private readonly IHostingEnvironment _appEnvironment;

        public VoiceActorsController(IHostingEnvironment appEnvironment)
        {
            _appEnvironment = appEnvironment;
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
        public async Task<ActionResult> Create([Bind("voiceActorImg,name,nameNative,surname,surnameNative")]
            VoiceActor voiceActor)
        {
            var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\voiceActorImg"),
                "voiceActorImg", HttpContext.Request.Form.Files);
            voiceActor.img = temp;

            VoiceActorDao voiceActorDao = new VoiceActorDao();
            try
            {
                voiceActorDao.Save(voiceActor);
            }
            catch (OracleException e)
            {
                Debug.WriteLine("Chyba uložení do databáze\n{0}", e);
                return View(voiceActor);
            }

            return RedirectToAction("Index", "Administration");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, string name)
        {
            VoiceActorDao voiceActorDao = new VoiceActorDao();
            VoiceActor voiceActor;

            try
            {
                voiceActor = voiceActorDao.FindById(id);
            }
            catch (Exception)
            {
                return NotFound();
            }

            return View(voiceActor);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,
            [Bind("voiceActorId,voiceActorImg,name,nameNative,surname,surnameNative")]
            VoiceActor voiceActor)
        {


            var temp = await Helper.UploadImg(Path.Combine(_appEnvironment.WebRootPath, "uploads\\voiceActorImg"),
                "voiceActorImg", HttpContext.Request.Form.Files);
            voiceActor.img = temp;

            VoiceActorDao voiceActorDao = new VoiceActorDao();

            voiceActorDao.Edit(voiceActor);
            return RedirectToAction("Index", "Administration");

        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {

            VoiceActorDao voiceActor = new VoiceActorDao();

            return View(voiceActor.FindById(id));
        }

        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id, VoiceActor voiceActor)
        {
            VoiceActorDao voiceActorDao = new VoiceActorDao();
            voiceActorDao.Delete(voiceActor);

            return RedirectToAction("Index", "Administration");
        }
    }
}