using System;
using System.Collections.Generic;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public abstract class BaseDao<T>
    {
        public BaseDao()
        {
            Database.GetInstance().Open();
        }

        ~BaseDao()
        {
            Database.GetInstance().Close();
        }

        // Upravit názvy method
        // Případně domyslet další důležité metody
        public abstract void Save(T data);
        public abstract List<T> FindAll(int? page, int? limit = null);
        public abstract T FindById(int id);
        public abstract T FindById(string id);
        public abstract List<T> FindBy(string condition, string table);
        public abstract void Edit(T data);
        public abstract void Delete(T data);
        public abstract T Map(OracleDataReader dataReader);

        public int GetPage(int? page)
        {
            int start = 0;

            if (page == null || page == 1)
                start = 0;

            if (page > 1)
                start = Helper.mediaPerPage; // vynechává se 10 a vypíše se 10 entit

            return start * (Convert.ToInt32(page) - 1);
        }

        public int CountPages(string table)
        {
            return CountRows(table) / Helper.mediaPerPage;
        }

        public int CountRows(string table)
        {
            QueryBuilder qb2 = new QueryBuilder().Select("COUNT(*)").From(table);
            OracleDataReader dr2 = Database.GetInstance().CreateCommnad(qb2.GetQuery()).ExecuteReader();
            dr2.Read();
            return Convert.ToInt32(dr2.GetInt32(0));
        }
    }
}