using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoYobuDb.Models.Tables
{
    [Table("REVIEWS")]
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("REVIEW_ID")]
        public int reviewId { get; set; }

        [Required]
        [Column("CONTENT", TypeName = "Varchar2(4000)")]
        [StringLength(4000)]
        public string content { get; set; }

        [Required]
        [Column("SCORE", TypeName = "Number(3)")]
        [Range(0, 10)]
        public int score { get; set; }

        [Required]
        [Column("TITLE", TypeName = "Varchar2(50)")]
        [StringLength(50)]
        public string title { get; set; }

        [Required]
        [Column("MY_DATE", TypeName = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}", ApplyFormatInEditMode = true)]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime myDate { get; set; }

        // Foreign keys
        [ForeignKey("userId")]
        [Column("USER_ID")]
        public string userId { get; set; }

        public User user { get; set; }

        [ForeignKey("reviewsThreadId")]
        [Column("REVIEWS_THREAD_ID")]
        public int reviewsThreadId { get; set; }

        public ReviewsThread reviewsThread { get; set; }
    }
}