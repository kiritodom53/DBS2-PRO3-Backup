using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using Castle.Core.Internal;
using MoYobuDb.Models.Tables;
using Oracle.ManagedDataAccess.Client;

namespace MoYobuDb.Models.Database.Dao
{
    public class AnimeCharacterDao : BaseDao<AnimeCharacter>
    {
        private BaseDao<AnimeCharacter> _baseDaoImplementation;

        public override void Save(AnimeCharacter data)
        {
            // TODO: Udělat validace, aby se nemohli přidat duplikáty
            QueryBuilder qb = new QueryBuilder().Insert("ANIME_CHARACTERS").Values(new Dictionary<string, string>
            {
                {"ANIME_ID", data.animeId.ToString()}, {"CHARACTER_ID", data.characterId.ToString()}
            });

            // TODO: Dodělat validaci, zatím blbne při kontrole duplikace
            AnimeCharacter animeCharacterData = new AnimeCharacter();
            animeCharacterData.animeId = data.animeId;
            animeCharacterData.characterId = data.characterId;
            if (!isDuplicated(animeCharacterData))
            {
                throw new DuplicateNameException();
            }

            OracleCommand cmd = Database.GetInstance().CreateCommnad(qb.GetQuery());

            Debug.WriteLine("data.userNAME : {0}");
            Debug.WriteLine(data.animeId);
            Debug.WriteLine(data.characterId);
            cmd.Parameters.Add("ANIME_ID", data.animeId);
            cmd.Parameters.Add("CHARACTER_ID", data.characterId);

            Debug.WriteLine(cmd.CommandText);

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

        private bool isDuplicated(AnimeCharacter animeCharacterData)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select(
                    "ANIME_ID",
                    "CHARACTER_ID")
                .From("ANIME_CHARACTERS")
                .Where("ANIME_ID = " + animeCharacterData.animeId + " AND CHARACTER_ID = " +
                       animeCharacterData.characterId);
            //QueryBuilder qb = new QueryBuilder().Select("ANIME_ID", "ENGLISH_TITLE", "ROMAJI_TITLE", "STUDIO_ID").From("ANIMES a");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<AnimeCharacter> animeCharacters = new List<AnimeCharacter>();

            dr.Read();
            if (dr.IsNullOrEmpty())
            {
                return true;
            }

            return false;
        }

        public override void Delete(AnimeCharacter data)
        {
            throw new NotImplementedException();
        }

        public override void Edit(AnimeCharacter data)
        {
            throw new NotImplementedException();
        }

        public override List<AnimeCharacter> FindAll(int? page = null, int? limit = null)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select(
                    "as.ANIME_CHARACTERS_ID",
                    "as.ANIME_ID",
                    "as.CHARACTER_ID",
                    "a.NAME")
                .From("ANIME_CHARACTERS as")
                .LeftJoin("ANIMES a", "a.ANIME_ID = as.ANIME_ID");
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<AnimeCharacter> animeCharacters = new List<AnimeCharacter>();

            while (dr.Read())
            {
                AnimeCharacter animeCharacter = this.Map(dr);
                animeCharacters.Add(animeCharacter);
            }

            return animeCharacters;
        }

        public override AnimeCharacter FindById(int id)
        {
            throw new NotImplementedException();
        }

        public override AnimeCharacter FindById(string id)
        {
            throw new NotImplementedException();
        }

        public override List<AnimeCharacter> FindBy(string condition, string table)
        {
            QueryBuilder qb = new QueryBuilder()
                .Select(
                    "ac.ANIME_CHARACTERS_ID, " +
                    "ac.ANIME_ID, " +
                    "ac.CHARACTER_ID, " +
                    "c.NAME")
                .From("ANIME_CHARACTERS ac")
                .LeftJoin("CHARACTERS c", "ac.CHARACTER_ID = c.CHARACTER_ID")
                .Where(condition);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();
            List<AnimeCharacter> animeCharacters = new List<AnimeCharacter>();


            while (dr.Read())
            {
                AnimeCharacter animeCharacter = this.Map(dr);
                Debug.WriteLine("__________________________________");
                animeCharacters.Add(animeCharacter);
            }

            return animeCharacters;
        }

        public override AnimeCharacter Map(OracleDataReader dataReader)
        {
            AnimeCharacter animeCharacter = new AnimeCharacter();
            try
            {
                animeCharacter.animeCharactersId = dataReader.GetInt32(0);
                animeCharacter.animeId = dataReader.GetInt32(1);
                animeCharacter.characterId = dataReader.GetInt32(2);

                Debug.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                Debug.WriteLine(animeCharacter.animeId);
                Debug.WriteLine(animeCharacter.animeCharactersId);

            }
            catch (IndexOutOfRangeException e)
            {
                Debug.WriteLine(e);
            }
            catch (InvalidCastException e)
            {
                Debug.WriteLine(e);
            }

            return animeCharacter;
        }
    }
}