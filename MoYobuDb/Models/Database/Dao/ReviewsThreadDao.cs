using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class ReviewsThreadDao : BaseDao<ReviewsThread>
    {
        public int rev_id { get; set; }

        public override void Save(ReviewsThread data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("REVIEWS_THREADS")
                .Values(new Dictionary<string, string> {{"REVIEWS_THREAD_ID", rev_id.ToString()}});

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            cmd.Parameters.Add("REVIEWS_THREAD_ID", rev_id.ToString());
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

        public override List<ReviewsThread> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override ReviewsThread FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override ReviewsThread FindById(string id)
        {
            throw new NotImplementedException();
        }

        public override List<ReviewsThread> FindBy(string condition, string table)
        {
            throw new System.NotImplementedException();
        }

        public override void Edit(ReviewsThread data)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(ReviewsThread data)
        {
            throw new System.NotImplementedException();
        }

        public override ReviewsThread Map(OracleDataReader dataReader)
        {
            throw new System.NotImplementedException();
        }
    }
}