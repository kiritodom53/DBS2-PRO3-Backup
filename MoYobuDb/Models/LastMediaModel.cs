using System.Collections.Generic;
using MoYobuDb.Models.Tables;

namespace MoYobuDb.Models
{
    public class LastMediaModel
    {
        public List<Anime> lastAnimes { get; set; }
        public List<Manga> lastMangas { get; set; }
    }
}