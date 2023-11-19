using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Gorev12.Models; 
using Gorev12.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace Gorev12.Controllers
{
    public class PostController : Controller
    {
        public IConfiguration Configuration;
        public AppDbContext DbContext;

        public PostController(IConfiguration configuration, AppDbContext dbContext)
        {
            Configuration = configuration;
            DbContext = dbContext;
        }

        public async Task<IActionResult> Index( string? search, string? searchCreatedBy, string? order, string? dir, int page = 1)
        {
            int pageSize = 10;

            IQueryable<PostEntity> query = DbContext.Posts
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(c => c.User);



            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Content.Contains(search) || p.Title.Contains(search));
                ViewBag.search = search;
            }

            if (!string.IsNullOrEmpty(searchCreatedBy))
            {
                query = query.Where(e => e.User.Username.Contains(searchCreatedBy));

                ViewBag.searchCreatedBy = searchCreatedBy;
            }



            switch (order)
            {
                case "Id":
                    query = dir == "asc" ? query.OrderBy(e => e.Id) : query.OrderByDescending(e => e.Id);
                    break;
                case "Title":
                    query = dir == "asc" ? query.OrderBy(e => e.Title) : query.OrderByDescending(e => e.Title);
                    break;
                case "Content":
                    query = dir == "asc" ? query.OrderBy(e => e.Content) : query.OrderByDescending(e => e.Content);
                    break;
                case "Comments":
                    query = dir == "asc" ? query.OrderBy(e => e.Comments.Count()).ThenBy(e => e.Id) : query.OrderByDescending(e => e.Comments.Count()).ThenByDescending(e => e.Id);
                    break;

                case "CreatedAt":
                    query = dir == "asc" ? query.OrderBy(e => e.CreatedAt) : query.OrderByDescending(e => e.CreatedAt);
                    break;
                case "CreatedBy":
                    query = dir == "asc" ? query.OrderBy(e => e.User.Username) : query.OrderByDescending(e => e.User.Username);
                    break;
                default:
                    break;
            }

            // Order ve directionları ViewBag ile View a gönderiyoruz.
            ViewBag.Order = order;
            ViewBag.Dir = dir;



            var createdByList = await DbContext.Posts.Select(e => e.User.Username)
                .OrderBy(e => e)
                .Distinct()
                .ToListAsync();

            //Toplam kayıt sayısını al
            int totalRecord = await query.CountAsync();

            // Toplam sayfa sayısını hesapla
            int totalPages = (int)Math.Ceiling((double)totalRecord / pageSize);

            page = Math.Max(1, Math.Min(page, totalPages));


            List<PostEntity> posts = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.createdByList = createdByList.Select(personName => new SelectListItem
            {
                Text = personName.ToString(),
                Selected = personName.ToString() == searchCreatedBy
            });

            // Sayfa bilgilerini ViewBag ile View a gönderiyoruz.
            ViewBag.Page = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalRecords = totalRecord;


            
            foreach (var post in posts)
            {
                var postComments = await DbContext.Comments
                    .Where(c => c.PostId == post.Id)
                    .Include(c => c.User) // Kullanıcı bilgisini de çekiyoruz
                    .ToListAsync();

                post.Comments = postComments;
                ViewBag.Comments = postComments;
            }
            

            return View(posts);

        }
    }
}