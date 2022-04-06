using BlogsEngine.Core.Interfaces;
using BlogsEngine.DAL.Interfaces;
using BlogsEngine.Models.Blogs;

namespace BlogsEngine.Core.Blogs
{
    public class BlogsService : IBlogsService
    {
        private readonly IBlogsRepository blogsRepository;

        public BlogsService(IBlogsRepository blogsRepository)
        {
            this.blogsRepository = blogsRepository;
        }

        public IEnumerable<Blog> GetBlogs()
        {
            var blogs = this.blogsRepository.GetAll();
            return blogs;
        }

        public IEnumerable<Blog> GetPendingBlogs()
        {
            var blogs = this.blogsRepository.GetPendingBlogs();
            return blogs;
        }

        public Blog PostBlog(Blog blog)
        {
            //run validator
            var result = this.blogsRepository.Add(blog);
            return result;
        }
        public Comment CommentBlog(int blogId, Comment comment)
        {
            //run validator
            var result = this.blogsRepository.CommentBlog(blogId, comment);
            return result;
        }

        public IEnumerable<Blog> GetMyOwnBlogs(int authorId)
        {
            var result = this.blogsRepository.GetMyOwnBlogs(authorId);
            return result;
        }

        public Blog PublishBlog(int id)
        {
            var result = this.blogsRepository.PublishBlog(id);
            return result;
        }

        public Blog RejectBlog(int id)
        {
            var result = this.blogsRepository.RejectBlog(id);
            return result;
        }

        public Blog PatchBlog(Blog blog)
        {
            var result = this.blogsRepository.Update(blog);
            return result;
        }
    }
}
