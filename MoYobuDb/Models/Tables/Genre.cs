using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("GENRES")]
    public class Genre
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("GENRE_ID")]
        public int genreId { get; set; }

        [Column("TITLE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        [Required]
        public string title { get; set; }


        public virtual ICollection<AnimeGenre> animeGenres { get; set; }
        public virtual ICollection<MangaGenre> mangaGenres { get; set; }
    }
}