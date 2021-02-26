using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("ANIME_GENRES")]
    public class AnimeGenre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ANIME_GENRE_ID")]
        public int animeGenreId { get; set; }

        [ForeignKey("animeId")]
        [Column("ANIME_ID")]
        public int animeId { get; set; }

        public virtual Anime anime { get; set; }

        [ForeignKey("genreId")]
        [Column("GENRE_ID")]
        public int genreId { get; set; }

        public virtual Genre genre { get; set; }
    }
}