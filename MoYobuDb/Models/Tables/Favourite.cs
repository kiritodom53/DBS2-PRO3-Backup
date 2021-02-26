namespace MoYobuDb.Models.Tables
{
    public class Favourite
    {
        public int favouriteId { get; set; }
        public int?  animeId { get; set; }
        public int?  mangaId { get; set; }
        public string userId { get; set; }
        public string type { get; set; }
    }
}