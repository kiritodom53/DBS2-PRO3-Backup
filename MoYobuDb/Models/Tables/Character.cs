using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MoYobuDb.Models.Tables
{
    [Table("CHARACTERS")]
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("CHARACTER_ID")]
        public int characterId { get; set; }

        [Required]
        [Column("NAME", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string name { get; set; }

        [Column("NAME_NATIVE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string nameNative { get; set; }

        [Column("SURNAME", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string surname { get; set; }

        [Column("SURNAME_NATIVE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string surnameNative { get; set; }

        [Column("BIRTHDATE", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? birthdate { get; set; }

        [Column("DESCRIPTION", TypeName = "Varchar2(4000)")]
        [StringLength(7000)]
        public string description { get; set; }

        [Column("LARGE_IMG_URL", TypeName = "Varchar2(200)")]
        public string largeImgUrl { get; set; }

        [Column("MEDIUM_IMG_URL", TypeName = "Varchar2(200)")]
        public string mediumImgUrl { get; set; }

        // Foreign Keys
        [ForeignKey("voiceActorId")]
        [Column("VOICE_ACTOR_ID")]
        public int? voiceActorId { get; set; }

        public VoiceActor voiceActor { get; set; }
        
        [Required]
        [Display(Name="File")]
        [NotMapped]
        public IFormFile characterImg { get; set; }


        public virtual ICollection<AnimeCharacter> animeCharacters { get; set; }
        public virtual ICollection<MangaGenre> mangaGenres { get; set; }
    }
}