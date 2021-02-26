using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("ANIME_STAFFS")]
    public class AnimeStaff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ANIME_STAFF_ID")]
        public int animeStaffId { get; set; }

        [ForeignKey("animeId")]
        [Column("ANIME_ID")]
        public int animeId { get; set; }

        public Anime anime { get; set; }

        [ForeignKey("staffId")]
        [Column("STAFF_ID")]
        public int staffId { get; set; }

        public Staff staff { get; set; }
    }
}