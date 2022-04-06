using BlogsEngine.Models.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsEngine.DAL.Interfaces
{
    public interface IBlogsRepository: IRepository<Blog>
    {
        IEnumerable<Blog> GetPendingBlogs();
        IEnumerable<Blog> GetMyOwnBlogs(int authorId);
        Blog PublishBlog(int id);
        Blog RejectBlog(int id);
        Comment CommentBlog(int blogId, Comment comment);
    }
}
