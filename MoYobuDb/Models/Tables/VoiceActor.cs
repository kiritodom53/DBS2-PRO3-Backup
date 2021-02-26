using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MoYobuDb.Models.Tables
{
    [Table("VOICE_ACTORS")]
    public class VoiceActor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("VOICE_ACTOR_ID")]
        public int voiceActorId { get; set; }

        [Column("BIRTH_PLACE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string birthPlace { get; set; }

        [Column("NAME", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        [Required]
        public string name { get; set; }

        [Column("NAME_NATIVE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        [Required]
        public string nameNative { get; set; }

        [Column("SURNAME", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        [Required]
        public string surname { get; set; }

        [Column("SURNAME_NATIVE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        [Required]
        public string surnameNative { get; set; }

        [Column("BIRTHDATE", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? birthdate { get; set; }
        
        public string img { get; set; }
        
        [Required]
        [Display(Name="File")]
        [NotMapped]
        public IFormFile voiceActorImg { get; set; }

        public virtual ICollection<Character> characters { get; set; }
    }
}