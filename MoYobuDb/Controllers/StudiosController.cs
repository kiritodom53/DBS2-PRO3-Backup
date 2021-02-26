using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoYobuDb.Models.Database.Dao;
using MoYobuDb.Models.Tables;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Controllers
{
    public class StudiosController : Controller
    {
        // Slouží pouze jako ukázková třída pro práci s CRUD funkcemi

        // GET: Studios
        [HttpGet]
        public IActionResult Index()
        {
            StudioDao studioDao = new StudioDao();
            // System.Diagnostics.Debug.WriteLine("   Výpis z datbáze");
            // foreach (var item in studioDao.FindAll())
            // {
            //     System.Diagnostics.Debug.WriteLine("   {0}", item.name);
            // }
            return View(studioDao.FindAll());
        }

        [HttpGet]
        [Route("API/GET/STUDIO/ALL")]
        public IActionResult FindAllJson()
        {
            StudioDao studioDao = new StudioDao();
            // System.Diagnostics.Debug.WriteLine("   Výpis z datbáze");
            // foreach (var item in studioDao.FindAll())
            // {
            //     System.Diagnostics.Debug.WriteLine("   {0}", item.name);
            // }

            return Content(JsonConvert.SerializeObject(studioDao.FindAll()), "application/json");
        }

        // GET: Studios/Details/5
        [HttpGet]
        public IActionResult Details(int id)
        {
            StudioDao studioDao = new StudioDao();
            AnimeDao animeDao = new AnimeDao();
            Debug.WriteLine("   anime by id {0}", studioDao.FindById(id).name);

            Studio studio = studioDao.FindById(id);
            //studio.studioAnime = animeDao.FindBy("studio_id = " + id, null);
            return View(studio);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Administration/Studio/Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Studios/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Route("Administration/Studio/Create")]
        public IActionResult Create([Bind("studioId,name")] Studio studioData)
        {
            //TODO: Nejde vytvořit nový studio
            if (ModelState.IsValid)
            {
                Studio studio = new Studio {name = studioData.name};
                StudioDao studioDao = new StudioDao();
                try
                {
                    studioDao.Save(studio);
                }
                catch (OracleException e)
                {
                    Debug.WriteLine("Chyba uložení do databáze\n{0}", e);
                    return View(studioData);
                }
            }

            return RedirectToAction("Index", "Administration");
            //return RedirectToAction(nameof(Index));
        }

        // GET: Studios/Edit/5
        //[Route("Studio/Edit/ID/{id:int}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            StudioDao studioDao = new StudioDao();

            Studio s;
            try
            {
                s = studioDao.FindById(id);
            }
            catch (Exception e)
            {
                return NotFound();
            }

            return View(s);
        }

        // POST: Studios/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        [Authorize]
        public IActionResult Edit(int id, [Bind("studioId,name")] Studio studioData)
        {
            int id1 = id;
            int id2 = studioData.studioId;
            if (id != studioData.studioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Studio studio = new Studio {name = studioData.name, studioId = id};

                StudioDao studioDao = new StudioDao();

                studioDao.Edit(studio);
                return RedirectToAction("Index", "Administration");
            }

            //return RedirectToAction(nameof(Index));
            return View(studioData);
        }

        // GET: Studios/Delete/5
        //[Route("Studio/Delete/ID/{id:int}")]
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            StudioDao studioDao = new StudioDao();

            return View(studioDao.FindById(id));
            //return View(Studio);
        }

        // POST: Studios/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Studio studioData)
        {
            StudioDao studioDao = new StudioDao();
            studioDao.Delete(studioData);

            return RedirectToAction("Index", "Administration");
        }
    }
}