using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("MANGA_CHARACTERS")]
    public class MangaCharacter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("MANGA_CHARACTER_ID")]
        public int mangaCharacterId { get; set; }

        [ForeignKey("mangaId")]
        [Column("MANGA_ID")]
        public int mangaId { get; set; }

        public Manga manga { get; set; }

        [ForeignKey("characterId")]
        [Column("CHARACTER_ID")]
        public int characterId { get; set; }

        public Character character { get; set; }
    }
}