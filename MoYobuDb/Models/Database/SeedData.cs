using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MoYobuDb.Data;
using MoYobuDb.Models.Tables;

namespace MoYobuDb.Models.Database
{
    public static class SeedData
    {
        /*private static async Task<Anime> AnimeInsert(int id, string studio)
        {
            QueryBuilder qb = new QueryBuilder().Select("STUDIO_ID").From("STUDIOS").Where("NAME = " + studio);
            OracleDataReader dr = Database.GetInstance().CreateCommnad(qb.GetQuery()).ExecuteReader();

            int std = dr.GetInt32(0);
            
            AnilistClient client = new AnilistClient();
            IMedia a = await client.GetMediaAsync(id);
            Anime anime = new Anime
            {
                animeId = 1,
                romajiTitle = a.DefaultTitle,
                englishTitle = a.EnglishTitle,
                japaneseTitle = a.NativeTitle,
                description = a.Description,
                coverImage = a.CoverImage,
                episodes = a.Episodes,
                episodeDuration = a.Duration,
                averageScore = a.Score,
                status = a.Status,
                anilistId = Convert.ToInt32(a.Url.Split('/')[4]),
                studioId = std
            };
            return anime;
        }

        public static async Task<List<Anime>> Anime(string studio, params int[] args)
        {
            List<Anime> l = new List<Anime>();
            for (int i = 0; i < args.Length; i++)
            {
                l.Add(await AnimeInsert(args[i], studio));
            }

            return l;
        }*/
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationDbContext>>()))
            {
                if (context.studios.Any())
                {
                    //return;
                }

                //Anime("MADHOUSE", 14345, 13271, 20458, 20769, 19815, 20832, 21386, 21416, 21410, 21875, 98437, 101474, 99426);
                //Anime("J.C.Staff", 109963, 101167, 102976, 97668, 99255, 21696, 21518, 20920, 13759, 6773);
                //Anime("Kyoto Animation", 20954, 21827, 98338, 21460, 18671, 12189, 9617, 7791, 4181, 2167, 1530);

                context.studios.AddRange(
                    new Studio {studioId = 1, name = "J.C.Staff"});

                context.studios.AddRange(
                    new Studio {studioId = 2, name = "Kyoto Animation"});

                context.studios.AddRange(
                    new Studio {studioId = 3, name = "MADHOUSE"});

                context.studios.AddRange(
                    new Studio {studioId = 4, name = "SILVER LINK"});

                context.studios.AddRange(
                    new Studio {studioId = 5, name = "Toei Animations"});

                context.studios.AddRange(
                    new Studio {studioId = 6, name = "MAPPA"});

                context.studios.AddRange(
                    new Studio {studioId = 7, name = "A-1 Pictures"});

                context.studios.AddRange(
                    new Studio {studioId = 8, name = "Bones"});

                context.studios.AddRange(
                    new Studio {studioId = 9, name = "Studio Deen"});

                context.studios.AddRange(
                    new Studio {studioId = 10, name = "White Fox"});

                /*foreach (var item in await Anime("MADHOUSE", 14345, 13271, 20458, 20769, 19815, 20832, 21386, 21416, 
                    21410, 21875, 98437, 101474, 99426))
                {
                    context.animes.AddRange(item);
                }*/

                context.SaveChanges();
            }
        }
    }
}