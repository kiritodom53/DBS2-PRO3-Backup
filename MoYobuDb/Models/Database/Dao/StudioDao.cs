using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    class StudioDao : BaseDao<Studio>
    {
        private readonly AnimeDao animeDao = new AnimeDao();

        // Ukázkové dao pro práci s dtb CRUD
        public override void Delete(Studio data)
        {
            QueryBuilder qb = new QueryBuilder().Delete().From("STUDIOS").Where("STUDIO_ID = " + data.studioId);
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

        public override List<Studio> FindAll(int? page = null, int? limit = null)
        {
            QueryBuilder qb;
            if (limit == 0)
            {
                qb = new QueryBuilder().Select("STUDIO_ID", "NAME").From("STUDIOS")
                    .OrderBy("NAME", SortingType.Asc);

            }
            else
            {
                qb = new QueryBuilder().Select("STUDIO_ID", "NAME").From("STUDIOS")
                    .OrderBy("NAME", SortingType.Asc)
                    .Offset(GetPage(page), Helper.mediaPerPage);

            }
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Studio> studios = new List<Studio>();

            while (dr.Read())
            {
                studios.Add(this.Map(dr));
            }

            return studios;
        }

        public override Studio FindById(int id)
        {
            QueryBuilder qb = new QueryBuilder().Select("*").From("STUDIOS").Where("STUDIO_ID = " + id);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            Studio studio = this.Map(dr);
            List<Anime> animes = animeDao.FindBy("a.STUDIO_ID = " + studio.studioId, null);
            studio.animes = animes;
            return studio;
        }

        public override Studio FindById(string id)
        {
            throw new NotImplementedException();
        }

        public override List<Studio> FindBy(string condition, string table)
        {
            QueryBuilder qb = new QueryBuilder().Select("studio_id, name")
                .From("STUDIOS");

            qb.OrderBy("name", SortingType.Asc)
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Studio> studios = new List<Studio>();
            
            while (dr.Read())
            {
                Studio studio = new Studio();
                studio.studioId = dr.GetInt32(0);
                studio.name = dr.GetString(1);
                

                studios.Add(studio);
            }

            return studios;
        }

        public override Studio Map(OracleDataReader dataReader)
        {
            Studio studio = new Studio();
            try
            {
                studio.studioId = dataReader.GetInt32(0);
                studio.name = dataReader.GetString(1);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return studio;
        }

        public override void Save(Studio data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("STUDIOS")
                .Values(new Dictionary<string, string> {{"NAME", data.name}});

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            cmd.Parameters.Add("NAME", data.name);

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

        public override void Edit(Studio data)
        {
            QueryBuilder qb = new QueryBuilder().Update("STUDIOS")
                .Set(new Dictionary<string, string> {{"NAME", data.name}}).Where("STUDIO_ID = " + data.studioId);

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
    }
}