using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class MyListDao : BaseDao<MyList>
    {
        public string listName { get; set; }
        public string listType { get; set; }
        public bool mapAnime = false;
        public bool mapManga = false;
        public string userId;
        public string userName;
        
        public MyListDao()
        {
        }
        
        public MyListDao(string listName, string listType, string userId, string userName)
        {
            this.listName = listName;
            this.listType = listType;
            this.userId = userId;
            this.userName = userName;
        }

        public override void Save(MyList data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("LISTS")
                .Values(new Dictionary<string, string>
                {
                    {"NAME", data.name},
                    {"USER_ID", data.userId},
                    {"TYPE", data.type},
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

        public  List<MyList> FindAllById(string conditions)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("l.list_id, l.name, l.type")
                .From("LISTS l")
                .Where(conditions);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<MyList> myLists = new List<MyList>();
            
            while (dr.Read())
            {
                MyList myList = this.Map(dr);
                myLists.Add(myList);
            }

            return myLists;
        }
        
        public override List<MyList> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override MyList FindById(int id)
        {
            throw new System.NotImplementedException();
        }

        public override MyList FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public override List<MyList> FindBy(string condition, string table)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("l.list_id, l.name, l.type")
                .From("LISTS l")
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<MyList> myLists = new List<MyList>();

            //mapAnime = true;
            if (listType == "anime")
                mapAnime = true;
            else if (listType == "manga")
                mapManga = true;
            while (dr.Read())
            {
                MyList myList = this.Map(dr);
                myLists.Add(myList);
            }

            return myLists;
        }

        public override void Edit(MyList data)
        {
            throw new System.NotImplementedException();
        }

        public override void Delete(MyList data)
        {
            throw new System.NotImplementedException();
        }

        public override MyList Map(OracleDataReader dataReader)
        {
            MyList myList = new MyList();
            AnimeListDao ald = new AnimeListDao();
            MangaListDao mld = new MangaListDao();
            UserDao userDao = new UserDao();
            try
            {
                myList.listId = dataReader.GetInt32(0);
                myList.name = dataReader.GetString(1);
                myList.type = dataReader.GetString(2);
                if (mapAnime)
                    if (listName.ToLower() == dataReader.GetString(1).ToLower() || listName.ToLower() == "all")
                        myList.animeLists = ald.FindBy("LIST_ID = " + dataReader.GetInt32(0) + " AND al.USER_ID = '" + userDao.FindByName(userName).userAspId + "'");
                if (mapManga)
                    if (listName.ToLower() == dataReader.GetString(1).ToLower() || listName.ToLower() == "all")
                        myList.mangaLists = mld.FindBy("LIST_ID = " + dataReader.GetInt32(0) + " AND ml.USER_ID = '" + userDao.FindByName(userName).userAspId + "'");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return myList;
        }
    }
}