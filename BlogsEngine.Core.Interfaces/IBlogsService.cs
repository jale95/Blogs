namespace BlogsEngine.Core.Interfaces
{
    using BlogsEngine.Models.Blogs;

    public interface IBlogsService
    {
        public Blog PostBlog(Blog blog);
        public IEnumerable<Blog> GetBlogs();
        public IEnumerable<Blog> GetPendingBlogs();
        public Comment CommentBlog(int blogId, Comment comment);
        public IEnumerable<Blog> GetMyOwnBlogs(int authorId);
        public Blog PublishBlog(int id);
        public Blog RejectBlog(int id);
        public Blog PatchBlog(Blog blog);
     
    }
}
