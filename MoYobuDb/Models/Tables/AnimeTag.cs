using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("ANIME_TAGS")]
    public class AnimeTag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ANIME_TAG_ID")]
        public int animeTagId { get; set; }

        [ForeignKey("animeId")]
        [Column("ANIME_ID")]
        public int animeId { get; set; }

        public Anime anime { get; set; }

        [ForeignKey("tagId")]
        [Column("TAG_ID")]
        public int tagId { get; set; }

        public Tag tag { get; set; }
    }
}