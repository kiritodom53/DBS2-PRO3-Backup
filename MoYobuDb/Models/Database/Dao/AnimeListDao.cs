using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class AnimeListDao : BaseDao<AnimeList>
    {
        public override void Save(AnimeList data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("ANIME_LISTS")
                .Values(new Dictionary<string, string>
                {
                    {"ANIME_ID", data.animeId.ToString()},
                    {"LIST_ID", data.listId.ToString()},
                    {"USER_ID", data.userId},
                });

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());
            Debug.WriteLine(cmd.CommandText);

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

        public bool IsExist(int animeId, int listId, string userId)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("count(*)")
                .From("ANIME_LISTS")
                .Where("ANIME_ID = " + animeId + " AND LIST_ID = " + listId + " AND USER_ID = '" + userId + "'");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            int count = dr.GetInt32(0);
            if (count == 0)
                return false;
            return true;
        }

        public override List<AnimeList> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override AnimeList FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override AnimeList FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public override List<AnimeList> FindBy(string condition, string table = null)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("al.ANIME_ID")
                .From("ANIME_LISTS al")
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<AnimeList> animeLists = new List<AnimeList>();
            

            while (dr.Read())
            {
                AnimeList animeList = this.Map(dr);
                animeLists.Add(animeList);
            }

            return animeLists;
        }

        public override void Edit(AnimeList data)
        {
            QueryBuilder qb = new QueryBuilder().Update("ANIME_LISTS")
                .Set(new Dictionary<string, string>
                {
                    {"LIST_ID", data.listId.ToString()}
                })
                .Where("anime_id = " + data.animeId + "and user_id = '" + data.userId + "'");

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

        public override void Delete(AnimeList data)
        {
            QueryBuilder qb = new QueryBuilder()
                .Delete()
                .From("ANIME_LISTS")
                .Where("anime_id = " + data.animeId + " and user_id = '" + data.userId + "'");
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

        public override AnimeList Map(OracleDataReader dataReader)
        {
            AnimeList animeList = new AnimeList();
            AnimeDao animeDao = new AnimeDao();
            try
            {
                animeList.animeId = dataReader.GetInt32(0);
                animeList.anime = animeDao.FindById(dataReader.GetInt32(0));
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return animeList;
        }
    }
}