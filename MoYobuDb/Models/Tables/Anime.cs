using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MoYobuDb.Models.Tables
{
    [Table("ANIMES")]
    public class Anime
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ANIME_ID")]
        public int animeId { get; set; }

        [Required]
        [Column("ROMAJI_TITLE", TypeName = "Varchar2(200)")]
        [StringLength(200)]
        public string romajiTitle { get; set; }

        [Column("JAPANESE_TITLE", TypeName = "Varchar2(200)")]
        [StringLength(200)]
        public string japaneseTitle { get; set; }

        [Column("ENGLISH_TITLE", TypeName = "Varchar2(200)")]
        [StringLength(200)]
        public string englishTitle { get; set; }
        
        [Column("DESCRIPTION", TypeName = "Varchar2(2000)")]
        [StringLength(2000)]
        public string description { get; set; }

        [Required]
        [Column("COVER_IMAGE", TypeName = "Varchar2(200)")]
        public string coverImage { get; set; }

        [Column("FORMAT", TypeName = "Varchar2(20)")]
        [StringLength(20)]
        public string format { get; set; }

        [Column("EPISODES", TypeName = "Number(4)")]
        [Range(0, 9999)]
        public int? episodes { get; set; }

        [Column("EPISODE_DURATION", TypeName = "Number(3)")]
        [Range(0, 999)]
        public int? episodeDuration { get; set; } = 23;

        [Column("STATUS", TypeName = "Varchar2(25)")]
        [StringLength(25)]
        public string status { get; set; }

        [Column("START_DATE", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? startDate { get; set; }

        [Column("END_DATE", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? endDate { get; set; }

        [Column("SEASON", TypeName = "Varchar2(20)")]
        [StringLength(20)]
        public string season { get; set; } // winter, spring atd...

        [Column("AVERAGE_SCORE")] public int? averageScore { get; set; } = 0;

        [Column("SOURCE", TypeName = "Varchar2(20)")]
        [StringLength(20)]
        public string source { get; set; }

        [Column("HASHTAG", TypeName = "Varchar2(100)")]
        [StringLength(100)]
        public string hashtag { get; set; }

        [Column("ANILIST_ID")] public int? anilistId { get; set; }

        [Column("MAL_ID")] public int? malId { get; set; }

        [ForeignKey("studioId")]
        [Column("STUDIO_ID")]
        public int? studioId { get; set; }

        public Studio studio { get; set; }

        [ForeignKey("episodesThreadId")]
        [Column("EPISODES_THREAD_ID")]
        public int? episodesThreadId { get; set; }

        public EpisodesThread episodesThread { get; set; }

        [ForeignKey("reviewsThreadId")]
        [Column("REVIEWS_THREAD_ID")]
        public int? reviewsThreadId { get; set; }

        public ReviewsThread reviewsThread { get; set; }
        
        [Display(Name="File")]
        [NotMapped]
        public IFormFile animeImg { get; set; }

        //public virtual List<AnimeCharacter> animeCharacters { get; set; }
        public virtual List<Character> characters { get; set; }
        public virtual List<Review> reviews { get; set; }
        public virtual ICollection<AnimeGenre> animeGenres { get; set; }
        public virtual ICollection<AnimeList> animeLists { get; set; }
        public virtual List<AnimeStaff> animeStaffs { get; set; }
        public virtual List<Staff> staffs { get; set; }
        public virtual ICollection<AnimeTag> animeTags { get; set; }
    }
}