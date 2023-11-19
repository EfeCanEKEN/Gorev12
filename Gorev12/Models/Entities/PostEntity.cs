using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gorev12.Models.Entities
{
    public class PostEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string CoverImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        public int? CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public UserEntity User { get; set; }

        
        public ICollection<CommentEntity> Comments { get; set; } /*= new List<CommentEntity>();*/


       





    }
}
