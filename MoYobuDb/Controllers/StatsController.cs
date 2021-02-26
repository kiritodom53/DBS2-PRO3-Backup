using Microsoft.AspNetCore.Mvc;
using MoYobuDb.Models;
using MoYobuDb.Models.Database.Dao;
using Newtonsoft.Json;

namespace MoYobuDb.Controllers
{
    public class StatsController : Controller
    {
        public IActionResult Index()
        {
            return View(new ChartsMediaView());
        }

        [HttpGet]
        [Route("API/GET/CHARTS/MEDIA")]
        public IActionResult MediaCharts()
        {
            StatsDao statsDao = new StatsDao();
            
            return Content(JsonConvert.SerializeObject(statsDao.GetMediaCharts()), "application/json");
        }
        
        [HttpGet]
        [Route("API/GET/CHARTS/TOPGENRES")]
        public IActionResult TopGenres()
        {
            StatsDao statsDao = new StatsDao();
            
            return Content(JsonConvert.SerializeObject(statsDao.GetTopGenres()), "application/json");
        }

        [HttpGet]
        [Route("API/GET/CHARTS/ANIMEBYYEARS")]
        public IActionResult AnimeByYears()
        {
            StatsDao statsDao = new StatsDao();

            return Content(JsonConvert.SerializeObject(statsDao.GetAnimeByYears()), "application/json");
        }
    }
}