namespace MoYobuDb.Models.Tables.Charts
{
    public class UserAnimeStats
    {
        public int? tv { get; set; }
        public int? ova { get; set; }
        public int? movie { get; set; }
        public int? music { get; set; }
        public int? special { get; set; }
        public int? ona { get; set; }
        public int? tvShort { get; set; }

        public int? completedAnime { get; set; }
        public int? dropedAnime { get; set; }
        public int? completedEpisodes { get; set; }
        public double? watchedDays { get; set; }
        public double? planedDays { get; set; }
    }
}