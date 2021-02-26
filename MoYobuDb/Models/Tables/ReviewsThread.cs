using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("REVIEWS_THREADS")]
    public class ReviewsThread
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("REVIEWS_THREAD_ID")]
        public int reviewsThreadId { get; set; }

        public virtual ICollection<Review> reviews { get; set; }
    }
}