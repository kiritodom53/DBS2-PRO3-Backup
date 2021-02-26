using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("MANGA_GENRES")]
    public class MangaGenre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("MANGA_GENRE_ID")]
        public int mangaGenreId { get; set; }

        [ForeignKey("mangaId")]
        [Column("MANGA_ID")]
        public int mangaId { get; set; }

        public Manga manga { get; set; }

        [ForeignKey("genreId")]
        [Column("GENRE_ID")]
        public int genreId { get; set; }

        public Genre genre { get; set; }
    }
}