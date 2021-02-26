using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using MoYobuDb.Models.Tables.Charts;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class StatsDao
    {
        public StatsDao()
        {
            Database.GetInstance().Open();
        }

        public MediaCharts GetMediaCharts()
        {
            QueryBuilder qb = new QueryBuilder().Select("anime, manga").From("graf_medii");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            MediaCharts m = new MediaCharts();
            m.anime = dr.GetInt32(0);
            m.manga = dr.GetInt32(1);

            return m;
        }

        public List<TopGenresChart> GetTopGenres()
        {
            QueryBuilder qb = new QueryBuilder().Select("title, count").From("graff_top_genres");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            List<TopGenresChart> genresData = new List<TopGenresChart>();
            while (dr.Read())
            {
                TopGenresChart tgc = new TopGenresChart();
                tgc.title = dr.GetString(0);
                tgc.count = dr.GetInt32(1);
                genresData.Add(tgc);
            }

            return genresData;
        }

        public List<AnimeByYearsChart> GetAnimeByYears()
        {
            QueryBuilder qb = new QueryBuilder().Select("year, counter").From("GRAFF_ANIME_BY_YEARS");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            List<AnimeByYearsChart> animeByYearsCharts = new List<AnimeByYearsChart>();
            while (dr.Read())
            {
                AnimeByYearsChart abyc = new AnimeByYearsChart();
                abyc.year = dr.GetValue(0).ToString();
                abyc.counter = dr.GetInt32(1);
                animeByYearsCharts.Add(abyc);
            }

            return animeByYearsCharts;
        }

        public UserAnimeStats GetAnimeUserStats(string userId)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("get_anime_user_basic_data('" + userId + "'), get_anime_user_format('" + userId + "')")
                .From("dual");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            string[] basicAnimeData = dr.GetString(0).Split('-');
            string[] animeFormat = dr.GetString(1).Split('-');

            UserAnimeStats userAnimeStats = new UserAnimeStats();
            var temp = basicAnimeData[2];

            userAnimeStats.completedAnime = basicAnimeData[0] == "" ? 0 : Convert.ToInt32(basicAnimeData[0]);
            userAnimeStats.dropedAnime = basicAnimeData[1] == "" ? 0 : Convert.ToInt32(basicAnimeData[1]);
            userAnimeStats.completedEpisodes = basicAnimeData[2] == "" ? 0 : Convert.ToInt32(basicAnimeData[2]);
            userAnimeStats.watchedDays = basicAnimeData[3] == "" ? .0d : Convert.ToDouble(basicAnimeData[3]);
            userAnimeStats.planedDays = basicAnimeData[4] == "" ? .0d : Convert.ToDouble(basicAnimeData[4]);
            userAnimeStats.tv = animeFormat[0] == "" ? 0 : Convert.ToInt32(animeFormat[0]);
            userAnimeStats.ova = animeFormat[1] == "" ? 0 : Convert.ToInt32(animeFormat[1]);
            userAnimeStats.movie = animeFormat[2] == "" ? 0 : Convert.ToInt32(animeFormat[2]);
            userAnimeStats.music = animeFormat[3] == "" ? 0 : Convert.ToInt32(animeFormat[3]);
            userAnimeStats.special = animeFormat[4] == "" ? 0 : Convert.ToInt32(animeFormat[4]);
            userAnimeStats.ona = animeFormat[5] == "" ? 0 : Convert.ToInt32(animeFormat[5]);
            userAnimeStats.tvShort = animeFormat[6] == "" ? 0 : Convert.ToInt32(animeFormat[6]);

            return userAnimeStats;
        }

        public UserMangaStats GetMangaUserStats(string userId)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("get_manga_user_basic_data('" + userId + "'), get_manga_user_format('" + userId + "')")
                .From("dual");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            string[] basicMangaData = dr.GetString(0).Split('-');
            string[] mangaFormat = dr.GetString(1).Split('-');

            UserMangaStats userMangaStats = new UserMangaStats();
            //{
            try
            {
                userMangaStats.readManga = Convert.ToInt32(basicMangaData[0]);
                userMangaStats.dropedManga = Convert.ToInt32(basicMangaData[1]);
                userMangaStats.readChapters = Convert.ToInt32(basicMangaData[2]);
                userMangaStats.readChapters = Convert.ToInt32(basicMangaData[2]);
                userMangaStats.readDays = Convert.ToDouble(basicMangaData[3]);
                userMangaStats.planedDays = Convert.ToDouble(basicMangaData[4]);
                userMangaStats.novel = Convert.ToInt32(mangaFormat[0]);
                userMangaStats.manga = Convert.ToInt32(mangaFormat[1]);
                userMangaStats.oneShot = Convert.ToInt32(mangaFormat[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return userMangaStats;
        }
    }
}