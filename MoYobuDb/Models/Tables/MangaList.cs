using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("MANGA_LISTS")]
    public class MangaList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("MANGA_LIST_ID")]
        public int mangaListId { get; set; }

        [ForeignKey("mangaId")]
        [Column("MANGA_ID")]
        public int mangaId { get; set; }

        public Manga manga { get; set; }

        [ForeignKey("listId")]
        [Column("LIST_ID")]
        public int listId { get; set; }
        
        public string userId { get; set; }

        public MyList myList { get; set; }
    }
}