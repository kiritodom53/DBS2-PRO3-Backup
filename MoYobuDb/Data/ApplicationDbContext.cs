using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MoYobuDb.Models;
using MoYobuDb.Models.Tables;

namespace MoYobuDb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Přidání "tabulek" do databáze
        public DbSet<Anime> animes { get; set; }
        public DbSet<AnimeGenre> animeGenres { get; set; }
        public DbSet<AnimeCharacter> animeCharacters { get; set; }
        public DbSet<AnimeList> animeLists { get; set; }
        public DbSet<AnimeStaff> animeStaffs { get; set; }
        public DbSet<AnimeTag> animeTags { get; set; }
        public DbSet<Episode> episodes { get; set; }
        public DbSet<EpisodesThread> episodesThreads { get; set; }
        public DbSet<Genre> genres { get; set; }
        public DbSet<Character> characters { get; set; }
        public DbSet<Manga> mangas { get; set; }
        public DbSet<MangaGenre> mangaGenres { get; set; }
        public DbSet<MangaCharacter> mangaCharacters { get; set; }
        public DbSet<MangaList> mangaLists { get; set; }
        public DbSet<MangaStaff> mangaStaffs { get; set; }
        public DbSet<MangaTag> mangaTags { get; set; }
        public DbSet<MyList> lists { get; set; }
        public DbSet<Review> reviews { get; set; }
        public DbSet<ReviewsThread> reviewsThreads { get; set; }
        public DbSet<Staff> staffs { get; set; }
        public DbSet<Studio> studios { get; set; }
        public DbSet<Tag> tags { get; set; }
        public DbSet<User> users { get; set; }
        public DbSet<VoiceActor> voiceActors { get; set; }
    }
}
