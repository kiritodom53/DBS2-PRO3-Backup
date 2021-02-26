using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class CharacterDao : BaseDao<Character>
    {
        private string typeTable;

        public CharacterDao(string typeTable)
        {
            this.typeTable = typeTable;
        }

        public CharacterDao()
        {
        }

        public override void Save(Character data)
        {
            //characterImg,name,nameNative,surname,surnameNative,description,largeImgUrl,mediumImgUrl,voiceActorId
            QueryBuilder qb = new QueryBuilder().Insert("CHARACTERS")
                .Values(new Dictionary<string, string>
                {
                    {"NAME", data.name},
                    {"NAME_NATIVE", data.nameNative},
                    {"SURNAME", data.surname},
                    {"SURNAME_NATIVE", data.surnameNative},
                    {"DESCRIPTION", data.description.Replace("'", "''")},
                    {"MEDIUM_IMG_URL", data.mediumImgUrl}
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

        public override void Delete(Character data)
        {
            QueryBuilder qb = new QueryBuilder().Delete().From("CHARACTERS")
                .Where("CHARACTER_ID = " + data.characterId);
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

        public override void Edit(Character data)
        {
            Dictionary<string, string> temp = new Dictionary<string, string>();

            temp.Add("CHARACTER_ID", data.characterId.ToString());
            temp.Add("NAME", data.name);
            temp.Add("NAME_NATIVE", data.nameNative);
            temp.Add("SURNAME", data.surname);
            temp.Add("SURNAME_NATIVE", data.surnameNative);
            temp.Add("MEDIUM_IMG_URL", data.mediumImgUrl);
            temp.Add("DESCRIPTION", data.description);

            QueryBuilder qb = new QueryBuilder().Update("CHARACTERS")
                .Set(temp).Where("CHARACTER_ID = " + data.characterId);
            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());


            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (OracleException e)
            {
                throw;
            }
        }

        public override List<Character> FindAll(int? page = null, int? limit = null)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("CHARACTER_ID", "NAME", "SURNAME", "DESCRIPTION", "c.MEDIUM_IMG_URL")
                .From("CHARACTERS");
            System.Diagnostics.Debug.WriteLine("limit : {0}", limit);

            if (limit != null || limit > 0)
            {
                //qb.Offset(0, 10);
                System.Diagnostics.Debug.WriteLine("je offset");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("není offset");
            }

            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Character> characters = new List<Character>();

            while (dr.Read())
            {
                characters.Add(this.Map(dr));
            }

            return characters;
        }

        public override Character FindById(string id)
        {
            throw new NotImplementedException();
        }

        public override List<Character> FindBy(string condition, string tbl)
        {
            string sel = "c.CHARACTER_ID, " +
                         "c.NAME, " +
                         "c.SURNAME, " +
                         "c.DESCRIPTION, " +
                         "c.MEDIUM_IMG_URL, " +
                         "ac.CHARACTER_ID AS AC_CHAR_ID";

            if (typeTable == "ANIME_CHARACTERS")
            {
                sel += ", va.VOICE_ACTOR_ID, " +
                       "va.NAME, " +
                       "va.NAME_NATIVE, " +
                       "va.SURNAME, " +
                       "va.SURNAME_NATIVE, " +
                       "va.IMG";
            }

            QueryBuilder qb = new QueryBuilder()
                .Select(sel)
                .From("CHARACTERS c")
                .LeftJoin(tbl + " ac", "ac.CHARACTER_ID = c.CHARACTER_ID");

            if (typeTable == "ANIME_CHARACTERS")
                qb.LeftJoin("VOICE_ACTORS va", "va.VOICE_ACTOR_ID = ac.VOICE_ACTOR_ID");

            qb.Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<Character> characters = new List<Character>();


            while (dr.Read())
            {
                Character character = this.Map(dr);
                characters.Add(character);
            }

            return characters;
        }

        public override Character FindById(int id)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select("c.CHARACTER_ID, " +
                        "c.NAME, " +
                        "c.SURNAME, " +
                        "c.DESCRIPTION, " +
                        "c.MEDIUM_IMG_URL")
                .From("CHARACTERS c")
                // /.Custom("")
                .Where("c.CHARACTER_ID = " + id);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            dr.Read();
            Character c = this.Map(dr);
            // Character c = new Character
            // {
            //     characterId = dr.GetInt32(0),
            //     name = dr.GetString(1),
            //     nameNative = dr.GetString(2),
            //     surname = dr.GetString(3),
            //     surnameNative = dr.GetString(4),
            //     description = dr.GetString(5),
            //     mediumImgUrl = dr.GetString(6)
            //     
            // };
            return c;
        }

        public override Character Map(OracleDataReader dataReader)
        {
            Character character = new Character();
            VoiceActor va = new VoiceActor();
            try
            {
                character.characterId = dataReader.GetInt32(0);
                character.name = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
                character.surname = dataReader.IsDBNull(2) ? null : dataReader.GetString(2);
                character.description = dataReader.IsDBNull(3) ? null : dataReader.GetString(3);
                if (!dataReader.IsDBNull(4))
                    character.mediumImgUrl = dataReader.GetString(4);

                if (typeTable == "ANIME_CHARACTERS")
                {
                    if (!dataReader.IsDBNull(6))
                    {
                        character.voiceActorId = dataReader.GetInt32(6);
                        va.voiceActorId = dataReader.GetInt32(6);
                        if (!dataReader.IsDBNull(7))
                            va.name = dataReader.GetString(7);

                        if (!dataReader.IsDBNull(8))
                            va.nameNative = dataReader.GetString(8);

                        if (!dataReader.IsDBNull(9))
                            va.surname = dataReader.GetString(9);

                        if (!dataReader.IsDBNull(10))
                            va.surnameNative = dataReader.GetString(10);
                        if (!dataReader.IsDBNull(11))
                            va.img = dataReader.GetString(11);
                        character.voiceActor = va;
                    }
                }

                //character.surname = dataReader.GetString(2);
                //character.description = dataReader.GetString(3);
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine(e);
            }

            return character;
        }
    }
}