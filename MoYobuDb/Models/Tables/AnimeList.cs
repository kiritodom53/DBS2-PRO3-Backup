using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("ANIME_LISTS")]
    public class AnimeList
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ANIME_LIST_ID")]
        public int animeListId { get; set; }

        [ForeignKey("animeId")]
        [Column("ANIME_ID")]
        public int animeId { get; set; }

        public Anime anime { get; set; }

        [ForeignKey("listId")]
        [Column("LIST_ID")]
        public int listId { get; set; }
        
        
        public string userId { get; set; }

        public MyList myList { get; set; }
    }
}