using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class MangaDao : BaseDao<Manga>
    {
        private BrowseFilter _browseFilter;

        public MangaDao()
        {
        }

        public MangaDao(BrowseFilter browseFilter)
        {
            _browseFilter = browseFilter;
        }

        private readonly string[] selectInsertParams =
        {
            "m.MANGA_ID", "m.ROMAJI_TITLE", "m.JAPANESE_TITLE", "m.ENGLISH_TITLE", "m.DESCRIPTIONS", "m.IMG",
            "m.FORMAT", "m.CHAPTERS", "m.STATUS", "m.START_DATE", "m.END_DATE", "m.SCORE", "m.SOURCE",
            "m.ANILIST_ID", "m.MAL_ID", "m.REVIEWS_THREAD_ID"
        };


        public override void Save(Manga data)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            temp.Add("ENGLISH_TITLE", data.englishTitle);
            temp.Add("JAPANESE_TITLE", data.japaneseTitle);
            temp.Add("ROMAJI_TITLE", data.romajiTitle);
            temp.Add("FORMAT", data.format);
            temp.Add("CHAPTERS", data.chapters.GetValueOrDefault().ToString());
            temp.Add("ANILIST_ID", data.anilistId.ToString());
            temp.Add("MAL_ID", data.malId.ToString());
            temp.Add("SCORE", data.score.ToString());
            temp.Add("SOURCE", data.source);
            temp.Add("START_DATE", data.startDate.ToString().Split(' ')[0]);
            temp.Add("END_DATE", data.endDate.ToString().Split(' ')[0]);
            temp.Add("STATUS", data.status);
            temp.Add("DESCRIPTIONS", data.descriptions);
            temp.Add("IMG", data.img);

            QueryBuilder qb = new QueryBuilder().Insert("MANGAS")
                .Values(temp);

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            Console.WriteLine(cmd.CommandText);

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

        public string GetListName(int mangaId, string userId)
        {
            QueryBuilder qb = new QueryBuilder().Select("l.name")
                .From("lists l")
                .LeftJoin("manga_lists ml", "ml.list_id = l.list_id")
                .Where("user_id = '" + userId + "' AND manga_id = " + mangaId);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();
            string listName = dr.GetString(0);

            return listName;
        }

        public bool isInList(string userId, int mangaId)
        {
            QueryBuilder qb = new QueryBuilder().Select("count(*)")
                .From("manga_lists")
                .Where("user_id = '" + userId + "' AND manga_id = " + mangaId);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();
            int count = dr.GetInt32(0);

            if (count == 0)
                return false;

            return true;
        }

        public override List<Manga> FindAll(int? page, int? limit = null)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select(selectInsertParams)
                .From("MANGAS m")
                //.Custom(" ROWNUM <= 10 ")
                .OrderBy("m.ENGLISH_TITLE", SortingType.Asc)
                //.Custom(" OFFSET " + GetPage(page) + " ROWS FETCH NEXT 10 ROWS ONLY")
                .Offset(GetPage(page), Helper.mediaPerPage);

            if (_browseFilter != null)
            {
                qb.Where("m.format like '%" + _browseFilter.format + "%' and m.score like '%" + _browseFilter.score +
                         "%' and m.status like '%" + _browseFilter.status + "%'");
            }

            //QueryBuilder qb = new QueryBuilder().Select("ANIME_ID", "ENGLISH_TITLE", "ROMAJI_TITLE", "STUDIO_ID").From("ANIMES a");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Manga> mangas = new List<Manga>();
            while (dr.Read())
            {
                Manga manga = this.Map(dr);
                mangas.Add(manga);
            }

            return mangas;
        }

        public override Manga FindById(int id)
        {
            //QueryBuilder qb = new QueryBuilder().Select("ANIME_ID", "ENGLISH_TITLE", "ROMAJI_TITLE", "STUDIO_ID").From("ANIMES").Where("ANIME_ID = " + id);
            QueryBuilder qb = new QueryBuilder()
                .Select(selectInsertParams)
                .From("MANGAS m")
                // /.Custom("")
                .Where("m.MANGA_ID = " + id)
                .OrderBy("m.ROMAJI_TITLE", SortingType.Asc);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            Manga manga = this.Map(dr);
            return manga;
        }

        public override Manga FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public override List<Manga> FindBy(string condition, string table)
        {
            QueryBuilder qb = new QueryBuilder().Select(selectInsertParams)
                .From("MANGAS m");

            if (table == "favourite")
            {
                qb.LeftJoin("FAVOURITES f", "m.manga_id = f.manga_id");
            }

            qb.OrderBy("ROMAJI_TITLE", SortingType.Asc)
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Manga> mangas = new List<Manga>();
            while (dr.Read())
            {
                Manga manga = this.Map(dr);
                Debug.WriteLine("__________________________________");
                // System.Diagnostics.Debug.WriteLine(dr.GetInt32(3));
                // System.Diagnostics.Debug.WriteLine(dr.GetString(4));
                mangas.Add(manga);
            }

            return mangas;
        }

        public override void Edit(Manga data)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();

            temp.Add("MANGA_ID", data.mangaId.ToString());
            temp.Add("ROMAJI_TITLE", data.romajiTitle);
            temp.Add("JAPANESE_TITLE", data.japaneseTitle);
            temp.Add("ENGLISH_TITLE", data.englishTitle);
            temp.Add("DESCRIPTIONS", data.descriptions);
            temp.Add("IMG", data.img);
            temp.Add("FORMAT", data.format);
            temp.Add("CHAPTERS", data.chapters.ToString());
            temp.Add("STATUS", data.status);
            temp.Add("SCORE", data.score.ToString());
            temp.Add("SOURCE", data.source);
            if (data.anilistId != null)
                temp.Add("ANILIST_ID", data.anilistId.ToString());
            if (data.malId != null)
                temp.Add("MAL_ID", data.malId.ToString());

            QueryBuilder qb = new QueryBuilder().Update("MANGAS")
                .Set(temp).Where("MANGA_ID = " + data.mangaId);
            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());
            
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

        public override void Delete(Manga data)
        {
            QueryBuilder qb = new QueryBuilder().Delete().From("MANGAS").Where("MANGA_ID = " + data.mangaId);
            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

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

        public override Manga Map(OracleDataReader dataReader)
        {
            Manga manga = new Manga();
            try
            {
                manga.mangaId = dataReader.GetInt32(0);
                manga.romajiTitle = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
                manga.japaneseTitle = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                manga.englishTitle = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                manga.descriptions = dataReader.IsDBNull(4) ? null : dataReader.GetString(4);
                manga.img = dataReader.IsDBNull(5) ? null : dataReader.GetString(5);
                manga.format = dataReader.IsDBNull(6) ? null : dataReader.GetString(6);
                manga.chapters = dataReader.IsDBNull(7) ? (int?)null : dataReader.GetInt32(7);
                manga.status = dataReader.IsDBNull(8) ? null : dataReader.GetString(8);
                manga.startDate = null;
                manga.endDate = null;
                manga.score = dataReader.IsDBNull(11) ? (int?)null : dataReader.GetInt32(11);
                manga.source = dataReader.IsDBNull(12) ? null : dataReader.GetString(12);
                manga.anilistId = dataReader.IsDBNull(13) ? (int?)null : dataReader.GetInt32(13);
                manga.malId = dataReader.IsDBNull(14) ? (int?)null : dataReader.GetInt32(14);
                if (!dataReader.IsDBNull(15))
                    manga.reviewsThreadId = dataReader.GetInt32(15);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return manga;
        }

        public void EditThread(int animeId)
        {
            QueryBuilder qb = new QueryBuilder().Update("MANGAS")
                .Set(new Dictionary<string, string> {{"REVIEWS_THREAD_ID", animeId.ToString()}})
                .Where("manga_id = " + animeId);

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());
            
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
                .From("MANGAS")
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

        public List<string> FindStatus()
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("status")
                .From("MANGAS")
                .Where("status is not null")
                .GroupBy("status")
                .OrderBy("status", SortingType.Asc);


            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<string> formats = new List<string>();
            while (dr.Read())
            {
                formats.Add(dr.GetString(0));
            }

            return formats;
        }

        public List<string> FindScore()
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("score")
                .From("MANGAS")
                .Where("score is not null")
                .GroupBy("score")
                .OrderBy("score", SortingType.Asc);


            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<string> formats = new List<string>();

            while (dr.Read())
            {
                formats.Add(dr.GetInt32(0).ToString());
            }

            return formats;
        }

        public List<Manga> FindLast()
        {
            QueryBuilder qb = new QueryBuilder().Select(selectInsertParams)
                .From("MANGAS m")
                .OrderBy("m.manga_id", SortingType.Asc)
                .Where("rownum <= 12");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Manga> mangas = new List<Manga>();

            while (dr.Read())
            {
                Manga manga = this.Map(dr);
                mangas.Add(manga);
            }

            return mangas;
        }
    }
}