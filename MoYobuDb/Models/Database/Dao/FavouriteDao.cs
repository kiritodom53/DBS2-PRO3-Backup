using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class FavouriteDao : BaseDao<Favourite>
    {
        private string idType;
        public FavouriteDao()
        {
        }
        
        public FavouriteDao(string idType)
        {
            this.idType = idType;
        }

        public override void Save(Favourite data)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();
            if (data.animeId != null)
                temp.Add("ANIME_ID", data.animeId.ToString());
            else
                temp.Add("ANIME_ID", null);
            
            if (data.mangaId != null)
                temp.Add("MANGA_ID", data.mangaId.ToString());
            else
                temp.Add("MANGA_ID", null);
            
            temp.Add("USER_ID", data.userId);
            temp.Add("TYPE", data.type);
            
            QueryBuilder qb = new QueryBuilder().Insert("FAVOURITES")
                .Values(temp);

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
        
        public bool IsExist(int id, string userId, string typeId)
        {
            QueryBuilder qb = new QueryBuilder().Select("count(*)").From("FAVOURITES").Where(typeId + " = " + id + " AND USER_ID = '" + userId + "'");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            int count = dr.GetInt32(0);
            if (count == 0)
                return false;
            return true;
        }

        public override List<Favourite> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override Favourite FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override Favourite FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public override List<Favourite> FindBy(string condition, string table)
        {
            throw new System.NotImplementedException();
        }

        public override void Edit(Favourite data)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(Favourite data)
        {
            int? id = 0;
            if (idType == "ANIME_ID")
                id = data.animeId;
            if (idType == "MANGA_ID")
                id = data.mangaId;
            QueryBuilder qb = new QueryBuilder().Delete().From("FAVOURITES").Where(idType + " = " + id + " AND USER_ID = '" + data.userId + "'");
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

        public override Favourite Map(OracleDataReader dataReader)
        {
            throw new System.NotImplementedException();
        }
    }
}