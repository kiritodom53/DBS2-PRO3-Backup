using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class UserDao : BaseDao<User>
    {
        public override void Save(User data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("USERS")
                .Values(new Dictionary<string, string>
                {
                    {"USERNAME", data.username},
                    {"DESCRIPTION", data.desciption},
                    {"PROFILE_IMG", data.profileImg},
                    {"USER_ASP_ID", data.userAspId}
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

        public override List<User> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override User FindById(int id)
        { 
            QueryBuilder qb = new QueryBuilder().Select(
                    "USER_ID, USERNAME, DESCRIPTION, PROFILE_IMG, USER_ASP_ID, IS_REVIEWER, ASK_FOR_REVIEWER")
                .From("USERS")
                .Where("USER_ID = '" + id + "'");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            User user = this.Map(dr);
            
            return user;
        }

        public override User FindById(string id)
        {
            QueryBuilder qb = new QueryBuilder().Select(
                "USER_ID, USERNAME, DESCRIPTION, PROFILE_IMG, USER_ASP_ID, IS_REVIEWER, ASK_FOR_REVIEWER")
                .From("USERS")
                .Where("USER_ASP_ID = '" + id + "'");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            User user = this.Map(dr);
            
            return user;
        }

        public override List<User> FindBy(string condition, string table)
        {
            QueryBuilder qb = new QueryBuilder().Select(
                    "USER_ID, USERNAME, DESCRIPTION, PROFILE_IMG, USER_ASP_ID, IS_REVIEWER, ASK_FOR_REVIEWER")
                .From(table)
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<User> users = new List<User>();

            while (dr.Read())
            {
                User user = this.Map(dr);
                users.Add(user);
            }

            return users;
        }
        
        public User FindByName(string userName)
        {
            QueryBuilder qb = new QueryBuilder().Select(
                    "USER_ID, USERNAME, DESCRIPTION, PROFILE_IMG, USER_ASP_ID, IS_REVIEWER, ASK_FOR_REVIEWER")
                .From("USERS")
                .Where("USERNAME = '" + userName + "'");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            dr.Read();
            
            User user = this.Map(dr);

            return user;
        }
        
        public override void Edit(User data)
        {
            QueryBuilder qb = new QueryBuilder().Update("USERS")
                .Set(new Dictionary<string, string>
                {
                    {"DESCRIPTION", data.desciption}
                }).Where("USER_ASP_ID = '" + data.userAspId + "'");

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

        public void ReviewRights(string column, int userId)
        {
            QueryBuilder qb = new QueryBuilder().Update("USERS")
                .Set(new Dictionary<string, string>
                {
                    {column, "1"}
                }).Where("USER_ID = " + userId);

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
        
        public void EditImg(User user)
        {
            QueryBuilder qb = new QueryBuilder().Update("USERS")
                .Set(new Dictionary<string, string>
                {
                    {"PROFILE_IMG", user.profileImg}
                }).Where("USER_ASP_ID = '" + user.userAspId + "'");

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
            
            QueryBuilder qb2 = new QueryBuilder().Update("\"AspNetUsers\"")
                .Set(new Dictionary<string, string>
                {
                    {"\"ProfileImage\"", user.profileImg}
                }).Where("\"Id\" = '" + user.userAspId + "'");

            OracleCommand cmd2 = Database.GetInstance().CreateCommnad(qb2.GetQuery());


            try
            {
                cmd2.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
        }

        public override void Delete(User data)
        {
            throw new System.NotImplementedException();
        }

        public override User Map(OracleDataReader dataReader)
        {
            User user = new User();
            try
            {
                user.userId = dataReader.GetInt32(0);
                user.username = dataReader.GetString(1);
                user.desciption = dataReader.IsDBNull(2) ? null :  dataReader.GetString(2);
                user.profileImg = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                user.userAspId = dataReader.GetString(4);
                user.isReviewer = dataReader.GetBoolean(5);
                user.askForReviewer = dataReader.GetBoolean(6);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return user;
        }
    }
}