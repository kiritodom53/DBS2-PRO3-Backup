using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("EPISODES_THREADS")]
    public class EpisodesThread
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("EPISODES_THREAD_ID")]
        public int episodesThreadId { get; set; }

        public virtual ICollection<Episode> episodes { get; set; }
    }
}