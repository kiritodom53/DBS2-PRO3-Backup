using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class StaffDao : BaseDao<Staff>
    {
        public override void Save(Staff data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("STAFFS")
                .Values(new Dictionary<string, string>
                {
                    {"NAME", data.name},
                    {"NAME_NATIVE", data.nameNative},
                    {"SURNAME", data.surname},
                    {"SURNAME_NATIVE", data.surnameNative}
                });

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

        public override List<Staff> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override Staff FindById(int id)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("s.STAFF_ID", "s.NAME", "s.NAME_NATIVE", "s.SURNAME", "s.SURNAME_NATIVE")
                .From("STAFFS s")
                // /.Custom("")
                .Where("s.STAFF_ID = " + id);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();

            // Domapování
            Staff staff = this.Map(dr);
            return staff;
        }

        public override Staff FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public override List<Staff> FindBy(string condition, string tbl)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select(
                    "s.STAFF_ID, " +
                    "s.NAME, " +
                    "s.NAME_NATIVE, " +
                    "s.SURNAME, " +
                    "s.SURNAME_NATIVE")
                .From("STAFFS s")
                .LeftJoin(tbl + " a", "a.staff_id = s.staff_id")
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Staff> staffs = new List<Staff>();

            while (dr.Read())
            {
                Staff staff = this.Map(dr);
                Debug.WriteLine("__________________________________");
                staffs.Add(staff);
            }

            return staffs;
        }

        public override void Edit(Staff data)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();

            temp.Add("STAFF_ID", data.staffId.ToString());
            if (data.name != null)
                temp.Add("NAME", data.name);
            if (data.nameNative != null)
                temp.Add("NAME_NATIVE", data.nameNative);
            if (data.surname != null)
                temp.Add("SURNAME", data.surname);
            if (data.surnameNative != null)
                temp.Add("SURNAME_NATIVE", data.surnameNative);


            QueryBuilder qb = new QueryBuilder().Update("STAFFS")
                .Set(temp).Where("STAFF_ID = " + data.staffId);
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

        public override void Delete(Staff data)
        {
            QueryBuilder qb = new QueryBuilder().Delete().From("STAFFS").Where("STAFF_ID = " + data.staffId);
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

        public override Staff Map(OracleDataReader dataReader)
        {
            Staff s = new Staff();
            try
            {
                s.staffId = dataReader.GetInt32(0);
                s.name = dataReader.GetString(1);
                s.nameNative = dataReader.GetString(2);
                s.surname = dataReader.GetString(3);
                s.surnameNative = dataReader.GetString(4);

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return s;
        }
    }
}