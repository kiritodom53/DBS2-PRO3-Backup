using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("TAGS")]
    public class Tag
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("TAG_ID")]
        public int tagId { get; set; }

        [Column("TITLE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string title { get; set; }

        public virtual ICollection<AnimeTag> animeTags { get; set; }
        public virtual ICollection<MangaTag> mangaTags { get; set; }
    }
}