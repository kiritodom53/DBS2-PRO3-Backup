using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("STUDIOS")]
    public class Studio
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("STUDIO_ID")]
        public int studioId { get; set; }

        [Required]
        [Column("NAME", TypeName = "VARCHAR2(50)")]
        [StringLength(50)]
        public string name { get; set; }

        public List<Anime> animes { get; set; } = new List<Anime>();
        
        //public virtual List<Anime> studioAnime { get; set; }
    }
}