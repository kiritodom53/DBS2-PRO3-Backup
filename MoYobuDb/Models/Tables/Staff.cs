using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MoYobuDb.Models.Tables
{
    [Table("STAFFS")]
    public class Staff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("STAFF_ID")]
        public int staffId { get; set; }

        [Column("BIRTH_PLACE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string birthPlace { get; set; }

        [Column("BIRTHDAY", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? birthday { get; set; }

        [Column("NAME", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        [Required]
        public string name { get; set; }

        [Required]
        [Column("NAME_NATIVE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string nameNative { get; set; }

        [Column("SURNAME", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string surname { get; set; }

        [Column("SURNAME_NATIVE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string surnameNative { get; set; }
        
        [Required]
        [Display(Name="File")]
        [NotMapped]
        public IFormFile staffImg { get; set; }



        public virtual ICollection<AnimeStaff> animeStaffs { get; set; }
        public virtual ICollection<MangaStaff> mangaStaffs { get; set; }
    }
}