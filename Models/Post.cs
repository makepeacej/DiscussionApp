using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DiscussionApp.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Genre { get; set; } 

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Content { get; set; }

        public int? UserId { get; set; }

        public int? Likes { get; set; } = 0;
        public int? Dislikes { get; set; } = 0;
    }
}
