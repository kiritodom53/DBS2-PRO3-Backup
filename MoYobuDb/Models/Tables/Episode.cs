using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("EPISODES")]
    public class Episode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("EPISODE_ID")]
        public int episodeId { get; set; }

        [Column("TITLE", TypeName = "VARCHAR2(50)")]
        [DisplayName("Title")]
        [StringLength(50)]
        public string title { get; set; }

        [ForeignKey("episodeThreadId")]
        [Column("EPISODES_THREAD_ID")]
        public int episodeThreadId { get; set; }

        public EpisodesThread episodeThread { get; set; }
    }
}