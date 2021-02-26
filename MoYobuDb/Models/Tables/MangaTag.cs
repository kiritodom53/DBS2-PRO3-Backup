using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("MANGA_TAGS")]
    public class MangaTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("MANGA_TAG_ID")]
        public int mangaTagId { get; set; }

        [ForeignKey("mangaId")]
        [Column("MANGA_ID")]
        public int mangaId { get; set; }

        public Manga manga { get; set; }

        [ForeignKey("tagId")]
        [Column("TAG_ID")]
        public int tagId { get; set; }

        public Tag tag { get; set; }
    }
}