using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class AnimeDao : BaseDao<Anime>
    {
        // public AnimeDao()
        // {
        //     this.realations.Add(new Realation(RelationType.MANY_TO_ONE, new JoinColumn("STUDIO_ID", "STUDIO_ID"), "STUDIOS"));
        // }
        private BrowseFilter _browseFilter;

        public AnimeDao()
        {
        }

        public AnimeDao(BrowseFilter browseFilter)
        {
            _browseFilter = browseFilter;
        }

        private readonly string[] selectInsertParams =
        {
            "a.ANIME_ID", "a.ROMAJI_TITLE", "a.JAPANESE_TITLE", "a.ENGLISH_TITLE", "a.DESCRIPTION", "a.COVER_IMAGE",
            "a.FORMAT", "a.EPISODES", "a.EPISODE_DURATION", "a.STATUS", "a.START_DATE", "a.END_DATE", "a.SEASON",
            "a.AVERAGE_SCORE", "a.SOURCE", "a.HASHTAG", "a.ANILIST_ID", "a.MAL_ID", "a.STUDIO_ID",
            "a.EPISODES_THREAD_ID", "a.REVIEWS_THREAD_ID", "s.STUDIO_ID", "s.NAME AS STUDIO_NAME"
        };


        public override void Delete(Anime data)
        {
            QueryBuilder qb = new QueryBuilder().Delete().From("ANIMES").Where("ANIME_ID = " + data.animeId);
            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            Debug.WriteLine("dlt");
            Debug.WriteLine(qb.GetQuery());
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        // public Studio FindStudioByAnimeId(int animeId)
        // {
        //     QueryBuilder qb = new QueryBuilder().Select()
        // }

        // public List<Anime> FindBy(string condition)
        // {
        //     QueryBuilder qb = new QueryBuilder().Select("a.ANIME_ID", "a.ENGLISH_TITLE", "a.ROMAJI_TITLE",
        //             "s.STUDIO_ID", "s.NAME AS STUDIO_NAME")
        //         .From("ANIMES a")
        //         .LeftJoin("STUDIOS s", "a.STUDIO_ID = s.STUDIO_ID")
        //         .Where(condition);
        //     OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
        //     List<Anime> animes = new List<Anime>();
        //
        //     while (dr.Read())
        //     {
        //         Anime anime = this.Map(dr);
        //         System.Diagnostics.Debug.WriteLine("__________________________________");
        //         System.Diagnostics.Debug.WriteLine(dr.GetInt32(3));
        //         System.Diagnostics.Debug.WriteLine(dr.GetString(4));
        //         animes.Add(anime);
        //     }
        //
        //     return animes;
        // }

        // TODO: Dodělat pak i parametry jako rok apod.
        public override List<Anime> FindAll(int? page, int? limit = null)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select(selectInsertParams)
                .From("ANIMES a")
                //.Custom(" ROWNUM <= 10 ")
                .LeftJoin("STUDIOS s", "a.STUDIO_ID = s.STUDIO_ID")
                .OrderBy("a.ENGLISH_TITLE", SortingType.Asc)
                //.Custom(" OFFSET " + GetPage(page) + " ROWS FETCH NEXT 10 ROWS ONLY")
                .Offset(GetPage(page), Helper.mediaPerPage);

            if (_browseFilter != null)
            {
                qb.Where("a.format like '%" + _browseFilter.format + "%' and extract(year from a.start_date) like '%" +
                         _browseFilter.year + "%' and s.name like '%" + _browseFilter.studio + "%'");
            }


            //QueryBuilder qb = new QueryBuilder().Select("ANIME_ID", "ENGLISH_TITLE", "ROMAJI_TITLE", "STUDIO_ID").From("ANIMES a");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Anime> animes = new List<Anime>();
            Debug.WriteLine("\n");
            Debug.WriteLine(qb.GetQuery());
            Debug.WriteLine("\n");
            while (dr.Read())
            {
                Anime anime = this.Map(dr);
                animes.Add(anime);
            }

            return animes;
        }

        public bool isInList(string userId, int animeId)
        {
            QueryBuilder qb = new QueryBuilder().Select("count(*)")
                .From("anime_lists")
                .Where("user_id = '" + userId + "' AND anime_id = " + animeId);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            Debug.WriteLine(qb.GetQuery());

            dr.Read();
            int count = dr.GetInt32(0);

            if (count == 0)
                return false;

            return true;
        }

        public string GetListName(int animeId, string userId)
        {
            QueryBuilder qb = new QueryBuilder().Select("l.name")
                .From("lists l")
                .LeftJoin("anime_lists al", "al.list_id = l.list_id")
                .Where("user_id = '" + userId + "' AND anime_id = " + animeId);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            Debug.WriteLine(qb.GetQuery());

            dr.Read();
            string listName = dr.GetString(0);

            return listName;
        }

        public void EditThread(int animeId)
        {
            QueryBuilder qb = new QueryBuilder().Update("ANIMES")
                .Set(new Dictionary<string, string> {{"REVIEWS_THREAD_ID", animeId.ToString()}})
                .Where("anime_id = " + animeId);

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            Debug.WriteLine("\n\nqb.ToString() : {0}", qb.ToString());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public override void Edit(Anime data)
        {
            // animeId = data.animeId,
            // romajiTitle = data.romajiTitle,
            // japaneseTitle = data.japaneseTitle,
            // englishTitle = data.englishTitle,
            // description = data.description,
            // coverImage = data.coverImage,
            // format = data.format,
            // episodes = data.episodes,
            // episodeDuration = data.episodeDuration,
            // status = data.status,
            // season = data.season,
            // averageScore = data.averageScore,
            // source = data.source,
            // hashtag = data.hashtag,
            // anilistId = data.anilistId,
            // malId = data.malId,
            // studioId = data.studioId

            Dictionary<string, string> temp = new Dictionary<string, string>();

            temp.Add("ANIME_ID", data.animeId.ToString());
            temp.Add("ROMAJI_TITLE", data.romajiTitle);
            temp.Add("JAPANESE_TITLE", data.japaneseTitle);
            temp.Add("ENGLISH_TITLE", data.englishTitle);
            temp.Add("DESCRIPTION", data.description);
            if (data.coverImage != null)
                temp.Add("COVER_IMAGE", data.coverImage);
            temp.Add("FORMAT", data.format);
            temp.Add("EPISODES", data.episodes.ToString());
            temp.Add("EPISODE_DURATION", data.episodeDuration.ToString());
            temp.Add("STATUS", data.status);
            temp.Add("SEASON", data.season);
            temp.Add("AVERAGE_SCORE", data.averageScore.ToString());
            temp.Add("SOURCE", data.source);
            temp.Add("HASHTAG", data.hashtag);
            if (data.anilistId != null)
                temp.Add("ANILIST_ID", data.anilistId.ToString());
            if (data.malId != null)
                temp.Add("MAL_ID", data.malId.ToString());
            if (data.studioId != 0)
                temp.Add("STUDIO_ID", data.studioId.ToString());
            else
                temp.Add("STUDIO_ID", null);

            QueryBuilder qb = new QueryBuilder().Update("ANIMES")
                .Set(temp).Where("anime_id = " + data.animeId);
            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            Debug.WriteLine("\n\nqb.ToString() : {0}", qb.ToString());

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public override Anime Map(OracleDataReader dataReader)
        {
            Anime anime = new Anime();
            Studio studio = new Studio();
            // Review review = new Review();
            try
            {
                //TODO: Všude podmínka jestli je dtb prázdná
                Debug.WriteLine("anime-map");
                anime.animeId = dataReader.GetInt32(0);
                anime.romajiTitle = dataReader.GetString(1);
                anime.japaneseTitle = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                anime.englishTitle = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                anime.description = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                anime.coverImage = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                anime.format = dataReader.IsDBNull(6) ? null : dataReader.GetString(6);
                anime.episodes = dataReader.IsDBNull(7) ? (int?)null : dataReader.GetInt32(7);
                anime.episodeDuration = dataReader.IsDBNull(8) ? (int?)null : dataReader.GetInt32(8);
                anime.status = dataReader.IsDBNull(9) ? null : dataReader.GetString(9);
                anime.startDate = dataReader.IsDBNull(10) ? (DateTime?)null : dataReader.GetDateTime(10);
                anime.endDate = dataReader.IsDBNull(11) ? (DateTime?)null : dataReader.GetDateTime(11);
                anime.season = dataReader.IsDBNull(12) ? null : dataReader.GetString(12);
                anime.averageScore = dataReader.IsDBNull(13) ? (int?)null : dataReader.GetInt32(13);
                anime.source = dataReader.IsDBNull(14) ? null : dataReader.GetString(14);
                //anime.hashtag = dataReader.GetString(15);
                anime.hashtag = dataReader.IsDBNull(15) ? null :
                    dataReader.IsDBNull(15) ? "" : dataReader.GetString(15);
                //anime.anilistId = dataReader.GetInt32(16);
                anime.anilistId = dataReader.IsDBNull(16) ? (int?)null : dataReader.GetInt32(16);
                //anime.malId = dataReader.GetInt32(17);
                anime.malId = dataReader.IsDBNull(17) ? (int?)null : dataReader.GetInt32(17);
                //anime.studioId = dataReader.GetInt32(18);
                anime.studioId = dataReader.IsDBNull(18) ? (int?)null : dataReader.GetInt32(18);
                // TODO: Ošetřit InvalidCastException - IsDBNull()
                if (!dataReader.IsDBNull(19))
                    anime.episodesThreadId = dataReader.GetInt32(19);
                if (!dataReader.IsDBNull(20))
                    anime.reviewsThreadId = dataReader.GetInt32(20);
                // try
                // {
                //     anime.episodesThreadId = dataReader.GetInt32(19);
                //     anime.reviewsThreadId = dataReader.GetInt32(20);
                // }
                // catch (Exception e) { }
                //studio.studioId = dataReader.GetInt32(21);
                studio.name = dataReader.IsDBNull(18) ? null : dataReader.GetString(22);

                // if (!dataReader.IsDBNull(23))
                //     review.reviewId = dataReader.GetInt32(23);
                // if (!dataReader.IsDBNull(24))
                //     review.content = dataReader.GetString(24);
                // if (!dataReader.IsDBNull(25))
                //     review.score = dataReader.GetInt32(25);
                // if (!dataReader.IsDBNull(26))
                //     review.title = dataReader.GetString(26);
                // if (!dataReader.IsDBNull(27))
                //     review.myDate = dataReader.GetDateTime(27);
                // if (!dataReader.IsDBNull(28))
                //     review.reviewsThreadId = dataReader.GetInt32(28);
                // if (!dataReader.IsDBNull(29))
                //     review.userId = dataReader.GetString(29);

                //7
                //Debug.WriteLine(dataReader.GetString(12));
                //Debug.WriteLine(dataReader.GetString(22));
                //Debug.WriteLine(studio.name);

                //TODO: Kontrola výpisu
                //System.Diagnostics.Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                //System.Diagnostics.Debug.WriteLine(anime.romajiTitle);
                //System.Diagnostics.Debug.WriteLine(anime.studio.studioId);
                //System.Diagnostics.Debug.WriteLine(anime.studioId);

                anime.studio = studio;
                //studio.animes.Add(anime); // Není potřebné
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return anime;
        }

        public override Anime FindById(string id)
        {
            throw new NotImplementedException();
        }

        public override List<Anime> FindBy(string condition, string table)
        {
            QueryBuilder qb = new QueryBuilder().Select(selectInsertParams)
                .From("ANIMES a")
                .LeftJoin("STUDIOS s", "a.STUDIO_ID = s.STUDIO_ID");

            if (table == "favourite")
            {
                qb.LeftJoin("FAVOURITES f", "a.ANIME_ID = f.ANIME_ID");
            }


            qb.OrderBy("ROMAJI_TITLE", SortingType.Asc)
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Anime> animes = new List<Anime>();

            while (dr.Read())
            {
                Anime anime = this.Map(dr);
                animes.Add(anime);
            }

            return animes;
        }


        public override Anime FindById(int id)
        {
            //QueryBuilder qb = new QueryBuilder().Select("ANIME_ID", "ENGLISH_TITLE", "ROMAJI_TITLE", "STUDIO_ID").From("ANIMES").Where("ANIME_ID = " + id);
            QueryBuilder qb = new QueryBuilder()
                .Select(selectInsertParams)
                .From("ANIMES a")
                .LeftJoin("STUDIOS s", "a.STUDIO_ID = s.STUDIO_ID")
                // /.Custom("")
                .Where("a.ANIME_ID = " + id)
                .OrderBy("a.ENGLISH_TITLE", SortingType.Asc);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            Anime anime = this.Map(dr);

            CharacterDao characterDao = new CharacterDao("ANIME_CHARACTERS");
            anime.characters = characterDao.FindBy("ac.ANIME_ID = " + anime.animeId, "ANIME_CHARACTERS");

            return anime;
        }

        public override void Save(Anime data)
        {
            int? _studioId;
            if (data.studioId == 0)
                _studioId = null;
            else
                _studioId = data.studioId;
            QueryBuilder qb = new QueryBuilder().Insert("ANIMES")
                .Values(new Dictionary<string, string>
                {
                    {"ROMAJI_TITLE", data.romajiTitle},
                    {"JAPANESE_TITLE", data.japaneseTitle},
                    {"ENGLISH_TITLE", data.englishTitle},
                    {"DESCRIPTION", data.description},
                    {"COVER_IMAGE", data.coverImage},
                    {"FORMAT", data.format},
                    {"EPISODES", data.episodes.ToString()},
                    {"EPISODE_DURATION", data.episodeDuration.ToString()},
                    {"STATUS", data.status},
                    {"START_DATE", data.startDate.ToString().Split(' ')[0]},
                    {"END_DATE", data.endDate.ToString().Split(' ')[0]},
                    {"SEASON", data.season},
                    {"AVERAGE_SCORE", data.averageScore.ToString()},
                    {"SOURCE", data.source},
                    {"HASHTAG", data.hashtag},
                    {"ANILIST_ID", data.anilistId.ToString()},
                    {"MAL_ID", data.malId.ToString()},
                    {"STUDIO_ID", _studioId.ToString()}
                });

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            Console.WriteLine(cmd.CommandText);

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                throw;
            }
        }

        public List<string> FindFormats()
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("format")
                .From("ANIMES")
                .Where("format is not null")
                .GroupBy("format")
                .OrderBy("format", SortingType.Asc);


            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<string> formats = new List<string>();
            while (dr.Read())
            {
                formats.Add(dr.GetString(0));
            }

            return formats;
        }

        public List<string> FindYears()
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("extract(year from start_date)")
                .From("ANIMES")
                .Where("extract(year from start_date) is not null")
                .GroupBy("extract(year from start_date)")
                .OrderBy("extract(year from start_date)", SortingType.Desc);


            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<string> years = new List<string>();

            while (dr.Read())
            {
                years.Add(dr.GetInt32(0).ToString());
            }

            return years;
        }

        public List<Anime> FindLast()
        {
            QueryBuilder qb = new QueryBuilder().Select("a.ANIME_ID", "a.ROMAJI_TITLE", "a.JAPANESE_TITLE", "a.ENGLISH_TITLE", "a.DESCRIPTION", "a.COVER_IMAGE",
                    "a.FORMAT", "a.EPISODES", "a.EPISODE_DURATION", "a.STATUS", "a.START_DATE", "a.END_DATE", "a.SEASON",
                    "a.AVERAGE_SCORE", "a.SOURCE", "a.HASHTAG", "a.ANILIST_ID", "a.MAL_ID", "a.STUDIO_ID",
                    "a.EPISODES_THREAD_ID", "a.REVIEWS_THREAD_ID")
                .From("ANIMES a")
                .OrderBy("a.anime_id", SortingType.Asc)
                .Where("rownum <= 12");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Anime> animes = new List<Anime>();

            while (dr.Read())
            {
                Anime anime = this.Map(dr);
                animes.Add(anime);
            }

            return animes;
        }
    }
}