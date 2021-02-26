using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MoYobuDb.Models.Tables
{
    [Table("USERS")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("USER_ID")]
        public int userId { get; set; }

        [Column("USERNAME", TypeName = "Varchar2(20)")]
        [StringLength(20)]
        [Required]
        public string username { get; set; }

        [Column("DESCRIPTION", TypeName = "Varchar2(4000)")]
        [StringLength(4000)]
        public string desciption { get; set; }

        // [Column("PASSWORD", TypeName = "Varchar2(30)")]
        // [StringLength(30)]
        // [Required]
        // public string password { get; set; }

        // [EmailAddress]
        // [Required]
        // [Column("EMAIL")]
        // public string email { get; set; }
        //
        // [DefaultValue(false)]
        // [Column("VERIFED")]
        // public bool verified { get; set; } = false;
        //
        // [Column("ROLE", TypeName = "Varchar2(20)")]
        // [StringLength(20)]
        // [DefaultValue("user")]
        // public string role { get; set; } = "user";
        
        [Column("PROFILE_IMG", TypeName = "Varchar2(200)")]
        public string profileImg { get; set; }
        
        [Column("USER_ASP_ID", TypeName = "Nvarchar2(450)")]
        public string userAspId { get; set; }
        
        public bool isReviewer { get; set; }
        public bool askForReviewer { get; set; }
        
        
        [Required]
        [Display(Name="File")]
        [NotMapped]
        public IFormFile profileImgfile { get; set; }

        public virtual List<Anime> favouritesAnime { get; set; }
        public virtual List<Manga> favouritesManga { get; set; }

        public virtual List<Review> reviews { get; set; }
        public virtual ICollection<MyList> myLists { get; set; }
    }
}