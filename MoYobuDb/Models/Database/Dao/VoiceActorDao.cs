using System;
using System.Collections.Generic;
using System.Diagnostics;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class VoiceActorDao : BaseDao<VoiceActor>
    {
        public override void Save(VoiceActor data)
        {
            QueryBuilder qb = new QueryBuilder().Insert("VOICE_ACTORS")
                .Values(new Dictionary<string, string>
                {
                    {"NAME", data.name},
                    {"NAME_NATIVE", data.nameNative},
                    {"SURNAME", data.surname},
                    {"SURNAME_NATIVE", data.surnameNative},
                    {"IMG", data.img}
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

        public override List<VoiceActor> FindAll(int? page, int? limit = null)
        {
            throw new System.NotImplementedException();
        }

        public override VoiceActor FindById(int id)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("v.VOICE_ACTOR_ID", "v.NAME", "v.NAME_NATIVE", "v.SURNAME", "v.SURNAME_NATIVE")
                .From("VOICE_ACTORS v")
                // /.Custom("")
                .Where("v.VOICE_ACTOR_ID = " + id);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();
            //Debug.WriteLine(dr.GetString(5));

            // Domapování
            VoiceActor voiceActor = this.Map(dr);
            return voiceActor;
        }

        public override VoiceActor FindById(string id)
        {
            throw new System.NotImplementedException();
        }

        public override List<VoiceActor> FindBy(string condition, string table)
        {
            throw new System.NotImplementedException();
        }

        public override void Edit(VoiceActor data)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();

            temp.Add("VOICE_ACTOR_ID", data.voiceActorId.ToString());
            if (data.name != null)
                temp.Add("NAME", data.name);
            if (data.nameNative != null)
                temp.Add("NAME_NATIVE", data.nameNative);
            if (data.surname != null)
                temp.Add("SURNAME", data.surname);
            if (data.surnameNative != null)
                temp.Add("SURNAME_NATIVE", data.surnameNative);
            if (data.img != null)
                temp.Add("IMG", data.img);


            QueryBuilder qb = new QueryBuilder().Update("VOICE_ACTORS")
                .Set(temp).Where("VOICE_ACTOR_ID = " + data.voiceActorId);
            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            Debug.WriteLine("\n\nqb.ToString() : {0}", qb.ToString());

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

        public override void Delete(VoiceActor data)
        {
            QueryBuilder qb = new QueryBuilder().Delete().From("VOICE_ACTORS").Where("VOICE_ACTOR_ID = " + data.voiceActorId);
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

        public override VoiceActor Map(OracleDataReader dataReader)
        {
            VoiceActor voiceActor = new VoiceActor();
            try
            {
                voiceActor.voiceActorId = dataReader.GetInt32(0);
                voiceActor.name = dataReader.GetString(1);
                voiceActor.nameNative = dataReader.GetString(2);
                voiceActor.surname = dataReader.GetString(3);
                voiceActor.surnameNative = dataReader.GetString(4);

            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return voiceActor;
        }
    }
}