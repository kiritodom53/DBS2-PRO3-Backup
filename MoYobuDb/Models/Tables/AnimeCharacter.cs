using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("ANIME_CHARACTERS")]
    public class AnimeCharacter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ANIME_CHARACTERS_ID")]
        public int animeCharactersId { get; set; }

        [ForeignKey("animeId")]
        [Column("ANIME_ID")]
        public int animeId { get; set; }

        public Anime anime { get; set; }

        [ForeignKey("characterId")]
        [Column("CHARACTER_ID")]
        public int characterId { get; set; }

        public Character character { get; set; }
    }
}