using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MoYobuDb.Models.Tables
{
    [Table("LISTS")]
    public class MyList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("LIST_ID")]
        public int listId { get; set; }

        [Required]
        [Column("NAME", TypeName = "Varchar2(30)")]
        [StringLength(30)]
        public string name { get; set; }

        [Required]
        [ForeignKey("userId")]
        [Column("USER_ID")]
        public string userId { get; set; }
        
        [Required]
        [Column("type")]
        public string type { get; set; }

        public User user { get; set; }
        
        [NotMapped]
        public List<SelectListItem> types { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "anime", Text = "Anime" },
            new SelectListItem { Value = "manga", Text = "Manga" },
        };


        public virtual List<AnimeList> animeLists { get; set; }
        public virtual List<MangaList> mangaLists { get; set; }
    }
}