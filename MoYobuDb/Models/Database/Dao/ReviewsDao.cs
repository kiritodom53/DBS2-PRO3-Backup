using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class ReviewsDao : BaseDao<Review>
    {
        public int rev_id { get; set; }
        public override void Save(Review data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("REVIEWS")
                .Values(new Dictionary<string, string>
                {
                    {"REVIEW_ID", data.reviewId.ToString()},
                    {"CONTENT", data.content},
                    {"SCORE", data.score.ToString()},
                    {"TITLE", data.title},
                    {"MY_DATE", null},
                    {"USER_ID", data.userId},
                    {"REVIEWS_THREAD_ID", data.reviewsThreadId.ToString()}
                });

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

        public string FindThread(string table, string nameId, int id)
        {
            QueryBuilder qb = new QueryBuilder().Select("REVIEWS_THREAD_ID")
                .From(table)
                .Where(nameId + " = " + id);
            
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            int temp = 0;
            try
            {
                temp = dr.GetInt32(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            if (temp == 0 || temp == null)
            {
                return null;
            }

            return temp.ToString();
        }
        
        public bool FindThreadBool(string table, int id)
        {
            QueryBuilder qb = new QueryBuilder().Select("count(REVIEWS_THREAD_ID)")
                .From(table)
                .Where("REVIEWS_THREAD_ID = " + id);
            
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            int temp = 0;
            dr.Read();
            try
            {
                temp = dr.GetInt32(0);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                //throw;
            }
            if (temp == 0)
            {
                Debug.WriteLine("return false");
                return false;
            }
            Debug.WriteLine("return true");
            return true;
        }

        public List<Review> FindAllById(int id)
        {

            QueryBuilder qb = new QueryBuilder().Select(
                "r.REVIEW_ID, " +
                "r.CONTENT, " +
                "r.SCORE, " +
                "r.TITLE, " +
                "r.MY_DATE, " +
                "USER_ID")
                .From("REVIEWS r")
                .Where("REVIEWS_THREAD_ID = " + id);


            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Review> reviews = new List<Review>();

            while (dr.Read())
            {
                Review review = this.Map(dr);
                reviews.Add(review);
            }

            return reviews;
        }

        public override List<Review> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override Review FindById(int id)
        {
            QueryBuilder qb = new QueryBuilder().Select(
                    "r.REVIEW_ID, " +
                    "r.CONTENT, " +
                    "r.SCORE, " +
                    "r.TITLE, " +
                    "r.MY_DATE, " +
                    "r.USER_ID, "+
                    "u.\"UserName\" ")
                .From("REVIEWS r")
                .LeftJoin("\"AspNetUsers\" u", "u.\"Id\" = r.user_id ")
                .Where("REVIEW_ID = " + id);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            Review review = this.Map(dr);
            return review;
        }

        public override Review FindById(string id)
        {
            throw new NotImplementedException();
        }

        public override List<Review> FindBy(string condition, string table)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select(
                    "r.REVIEW_ID, " +
                    "r.CONTENT, " +
                    "r.SCORE, " +
                    "r.TITLE, " +
                    "r.MY_DATE, " +
                    "r.USER_ID, "+
                    "u.\"UserName\" ")
                .From("reviews r");
                
                if (table != null)
                    qb.LeftJoin(table + " a", "r.REVIEWS_THREAD_ID = a.REVIEWS_THREAD_ID");
                qb.LeftJoin("\"AspNetUsers\" u", "u.\"Id\" = r.user_id ")
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Review> reviews = new List<Review>();

            while (dr.Read())
            {
                Review review = this.Map(dr);
                Debug.WriteLine("__________________________________");
                reviews.Add(review);
            }

            return reviews;
        }

        public override void Edit(Review data)
        {
            QueryBuilder qb = new QueryBuilder().Update("REVIEWS")
                .Set(new Dictionary<string, string>
                    {
                        {"CONTENT", data.content},
                        {"SCORE", data.score.ToString()},
                        {"TITLE", data.title}
                    }
                ).Where("REVIEW_ID = " + data.reviewId);

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

        public override void Delete(Review data)
        {
            QueryBuilder qb = new QueryBuilder().Delete().From("REVIEWS").Where("REVIEW_ID = " + data.reviewId);
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

        public override Review Map(OracleDataReader dataReader)
        {
            Review r = new Review();
            
            try
            {
                UserDao userDao = new UserDao();
                r.reviewId = dataReader.GetInt32(0);
                r.content = dataReader.GetString(1);
                r.score = dataReader.GetInt32(2);
                r.title = dataReader.GetString(3);

                
                if (!dataReader.IsDBNull(5))
                {
                    r.userId = dataReader.GetString(5);
                    r.user = userDao.FindById(dataReader.GetString(5));
                }
                

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return r;
        }
    }
}