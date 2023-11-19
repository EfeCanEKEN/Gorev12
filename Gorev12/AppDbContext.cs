using Bogus;
using Gorev12.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Gorev12
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var users = new Faker<UserEntity>()
                .RuleFor(u => u.Id, f => f.IndexFaker + 1)
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Password, f => f.Internet.Password())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.CreatedAt, f => f.Date.Past())
                .Generate(5);

            modelBuilder.Entity<UserEntity>().HasData(users);

            var postFaker = new Faker<PostEntity>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Title, f => f.Lorem.Sentence())
                .RuleFor(p => p.Content, f => f.Lorem.Paragraph())
                .RuleFor(p => p.CoverImageUrl, f => f.Image.PicsumUrl())
                .RuleFor(p => p.CreatedAt, f => f.Date.Past())
                .RuleFor(p => p.CreatedBy, f => f.PickRandom(users).Id);

            var posts = postFaker.Generate(100);
            modelBuilder.Entity<PostEntity>().HasData(posts);

            var commentFaker = new Faker<CommentEntity>()
                .RuleFor(c => c.Id, f => f.IndexFaker + 1)
                .RuleFor(c => c.PostId, f => f.PickRandom(posts).Id)
                .RuleFor(c => c.Email, f => f.PickRandom(users).Email)
                .RuleFor(c => c.Content, f => f.Lorem.Sentence())
                .RuleFor(c => c.CreatedAt, f => f.Date.Past())
                .RuleFor(c => c.CreatedBy, f => f.PickRandom(users).Id);

            
            var comments = new List<CommentEntity>();

            foreach (var post in posts)
            {
                var commentCount = new Random().Next(3, 6);
                for (int i = 0; i < commentCount; i++)
                {
                    var comment = commentFaker.Generate();
                    comment.PostId = post.Id;
                    comments.Add(comment);
                }
            }

           

            modelBuilder.Entity<CommentEntity>().HasData(comments);

            
        }
    }
}
