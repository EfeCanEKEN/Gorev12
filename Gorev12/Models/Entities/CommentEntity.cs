//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;

//namespace Gorev12.Models.Entities
//{
//    public class CommentEntity
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int Id { get; set; }

//        [Required]
//        [EmailAddress]
//        public string Email { get; set; } = string.Empty;

//        [Required]
//        public string Content { get; set; } = string.Empty;

//        [Required]
//        public DateTime CreatedAt { get; set; }


//        [ForeignKey(nameof(User))]
//        public int? CreatedBy { get; set; }
//        public UserEntity User { get; set; }



//        [ForeignKey(nameof(Post))]
//        public int PostId { get; set; }
//        public PostEntity Post { get; set; }

//    }
//}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gorev12.Models.Entities
{
    public class CommentEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        
        public int? CreatedBy { get; set; }

        [ForeignKey(nameof(CreatedBy))]
        public UserEntity User { get; set; }

        // İlişkiyi belirtiyoruz
        public int PostId { get; set; }

        [ForeignKey(nameof(PostId))]
        public PostEntity Post { get; set; }
    }
}
