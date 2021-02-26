using System.Collections.Generic;

namespace MoYobuDb.Models.Tables
{
    public class AnimeDetailViewModel
    {
        public Anime anime { get; set; }
        public AnimeList animeList { get; } = new AnimeList();
    }
}