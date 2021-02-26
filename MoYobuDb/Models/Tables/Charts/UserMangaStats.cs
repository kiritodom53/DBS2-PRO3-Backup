namespace MoYobuDb.Models.Tables.Charts
{
    public class UserMangaStats
    {
        public int novel { get; set; }
        public int manga { get; set; }
        public int oneShot { get; set; }
        
        public int readManga { get; set; }
        public int dropedManga { get; set; }
        public int readChapters { get; set; }
        public double readDays { get; set; }
        public double planedDays { get; set; }
    }
}