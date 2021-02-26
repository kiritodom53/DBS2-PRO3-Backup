using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MoYobuDb.Models.Tables
{
    [Table("MANGAS")]
    public class Manga
    {
        // TODO: uspořádat ( trochu uspořádáno )
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("MANGA_ID")]
        public int mangaId { get; set; }

        [Column("ENGLISH_TITLE", TypeName = "Varchar2(200)")]
        [StringLength(200)]
        public string englishTitle { get; set; }

        [Column("JAPANESE_TITLE", TypeName = "Varchar2(200)")]
        [StringLength(200)]
        public string japaneseTitle { get; set; }

        [Required]
        [Column("ROMAJI_TITLE", TypeName = "Varchar2(200)")]
        [StringLength(200)]
        public string romajiTitle { get; set; }

        [Column("FORMAT", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string format { get; set; }

        [Column("CHAPTERS", TypeName = "Number(4)")]
        [Range(0, 9999)]
        public int? chapters { get; set; } = 0;
        
        [Column("ANILIST_ID")] 
        public int? anilistId { get; set; }

        [Column("MAL_ID")] 
        public int? malId { get; set; }

        [Column("SCORE", TypeName = "Number(3)")]
        [Range(0, 100)]
        public int? score { get; set; }
        
        [Column("SOURCE", TypeName = "Varchar2(20)")]
        [StringLength(20)]
        public string source { get; set; }

        [Column("START_DATE", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? startDate { get; set; }

        [Column("END_DATE", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? endDate { get; set; }
        
        public string descriptions { get; set; }
        public string img { get; set; }

        [Column("STATUS", TypeName = "Varchar2(25)")]
        [StringLength(25)]
        public string status { get; set; }

        // Foreign key

        [ForeignKey("reviewsThreadId")]
        [Column("REVIEWS_THREAD_ID")]
        public int? reviewsThreadId { get; set; }

        public ReviewsThread reviewsThread { get; set; }
        
        [Display(Name="File")]
        [NotMapped]
        public IFormFile mangaImg { get; set; }

        public virtual List<Character> characters { get; set; }
        public virtual List<Staff> staffs { get; set; }
        public virtual List<Review> reviews { get; set; }
        public virtual List<MangaCharacter> mangaCharacters { get; set; }
        public virtual ICollection<MangaGenre> mangaGenres { get; set; }
        public virtual ICollection<MangaList> mangaLists { get; set; }
        public virtual ICollection<MangaStaff> mangaStaffs { get; set; }
        public virtual ICollection<MangaTag> mangaTags { get; set; }
    }
}