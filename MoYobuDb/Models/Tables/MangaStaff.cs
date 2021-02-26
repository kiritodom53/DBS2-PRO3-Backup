using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("MANGA_STAFFS")]
    public class MangaStaff
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("MANGA_STAFF_ID")]
        public int mangaStaffId { get; set; }

        [ForeignKey("mangaId")]
        [Column("MANGA_ID")]
        public int mangaId { get; set; }

        public Manga manga { get; set; }

        [ForeignKey("staffId")]
        [Column("STAFF_ID")]
        public int staffId { get; set; }

        public Staff staff { get; set; }
    }
}