using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class MangaListDao : BaseDao<MangaList>
    {
        public override void Save(MangaList data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("MANGA_LISTS")
                .Values(new Dictionary<string, string>
                {
                    {"MANGA_ID", data.mangaId.ToString()},
                    {"LIST_ID", data.listId.ToString()},
                    {"USER_ID", data.userId}
                });

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

        public bool IsExist(int animeId, int listId)
        {
            QueryBuilder qb = new QueryBuilder().Select("count(*)").From("MANGA_LISTS").Where("MANGA_ID = " + animeId + " AND LIST_ID = " + listId);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            int count = dr.GetInt32(0);
            if (count == 0)
                return false;
            return true;
        }

        public override List<MangaList> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override MangaList FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override MangaList FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public override List<MangaList> FindBy(string condition, string table = null)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("ml.MANGA_ID")
                .From("MANGA_LISTS ml")
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<MangaList> mangaLists = new List<MangaList>();
            
            while (dr.Read())
            {
                MangaList mangaList = this.Map(dr);
                Debug.WriteLine("__________________________________");
                mangaLists.Add(mangaList);
            }

            return mangaLists;
        }

        public override void Edit(MangaList data)
        {
            QueryBuilder qb = new QueryBuilder().Update("MANGA_LISTS")
                .Set(new Dictionary<string, string>
                {
                    {"LIST_ID", data.listId.ToString()}
                })
                .Where("manga_id = " + data.mangaId + "and user_id = '" + data.userId + "'");

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

        public override void Delete(MangaList data)
        {
            QueryBuilder qb = new QueryBuilder()
                .Delete()
                .From("MANGA_LISTS")
                .Where("manga_id = " + data.mangaId + " and user_id = '" + data.userId + "'");
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

        public override MangaList Map(OracleDataReader dataReader)
        {
            MangaList mangaList = new MangaList();
            MangaDao mangaDao = new MangaDao();
            try
            {
                mangaList.mangaId = dataReader.GetInt32(0);
                mangaList.manga = mangaDao.FindById(dataReader.GetInt32(0));
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return mangaList;
        }
    }
}