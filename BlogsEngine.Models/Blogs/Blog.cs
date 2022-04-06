using BlogsEngine.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsEngine.Models.Blogs
{
    public class Blog
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? DateOfPublishing { get; set; }
        public int? AuthorId { get; set; }
        public int? Status { get; set; }
    }
}
