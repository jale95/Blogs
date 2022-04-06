using BlogsEngine.DAL.Interfaces;
using BlogsEngine.Models.Authentication;
using BlogsEngine.Models.Blogs;
using Dapper;
using Dapper.Mapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Z.Dapper.Plus;


namespace BlogsEngine.DAL
{
    public class BlogsSQLRepository : IBlogsRepository
    {
        private string connectionString;

        public BlogsSQLRepository()
        {
            this.connectionString = Environment.GetEnvironmentVariable("SQLConnectionString", EnvironmentVariableTarget.Process);
        }
        public Blog Add(Blog newObject)
        {
            string query = "INSERT INTO tblBlogs(Title, Content, DateOfPublishing, AuthorId, Status )" +
                " OUTPUT INSERTED.Id" +
                " VALUES(@title, @content, @dateOfPublishing, @authorId, 0)";
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@title", newObject.Title, DbType.AnsiString, ParameterDirection.Input, 30);
                parameter.Add("@content", newObject.Content, DbType.AnsiString, ParameterDirection.Input, 150);
                parameter.Add("@dateOfPublishing", newObject.DateOfPublishing, DbType.DateTime, ParameterDirection.Input, 150);
                parameter.Add("@authorId", newObject.AuthorId, DbType.AnsiString, ParameterDirection.Input, 150);
                parameter.Add("@Status", 2, DbType.Int32, ParameterDirection.Input);
                newObject.Id = db.QuerySingle<int>(query, parameter, commandType: CommandType.Text);
            }

            return newObject;
        }

        public Comment CommentBlog(int blogId, Comment comment)
        {
            string query = "INSERT INTO tblComments(Content, BlogId, AuthorId )" +
                " VALUES(@content, @blogId, @authorId)";

            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                DynamicParameters parameter = new DynamicParameters();

                parameter.Add("@content", comment.Content, DbType.AnsiString, ParameterDirection.Input, 200);
                parameter.Add("@authorId", comment.AuthorId, DbType.Int32, ParameterDirection.Input);
                parameter.Add("@blogId", comment.BlogId, DbType.Int32, ParameterDirection.Input);               
                comment.Id = db.QuerySingleOrDefault<int>(query, parameter, commandType: CommandType.Text);
            }

            return comment;
        }

        public Blog Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blog> GetAll()
        {
            IEnumerable<Blog> blogs;
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {

                string sql = "SELECT blog.Id, blog.Title, blog.Content, blog.DateOfPublishing, blog.AuthorId, blog.Status " +
                    "FROM tblBlogs blog " +
                    "WHERE blog.Status = 1";

                blogs = db.Query<Blog>(sql).ToList();              

                return blogs;
            }
        }
        public Blog GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Blog> GetMyOwnBlogs(int authorId)
        {
            IEnumerable<Blog> blogs;
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {

                string sql = "SELECT blog.Id, blog.Title, blog.Content, blog.DateOfPublishing, blog.AuthorId, blog.Status " +
                    "FROM tblBlogs blog " +
                    "WHERE blog.AuthorId = @authorId";

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@authorId", authorId, DbType.Int32, ParameterDirection.Input);
                blogs = db.Query<Blog>(sql, parameter, commandType: CommandType.Text).ToList();

                return blogs;
            }
        }

        public IEnumerable<Blog> GetPendingBlogs()
        {
            IEnumerable<Blog> blogs;
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {

                string sql = "SELECT blog.Id, blog.Title, blog.Content, blog.DateOfPublishing, blog.AuthorId, blog.Status " +
                    "FROM tblBlogs blog " +
                    "WHERE blog.Status = 2";

                blogs = db.Query<Blog>(sql).ToList();

                return blogs;
            }
        }

        public Blog PublishBlog(int id)
        {
            Blog blog;
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                string sql = "UPDATE tblBlogs SET Status = 1 " +
                    "WHERE Id = @id " +
                    "SELECT blog.Id, blog.Title, blog.Content, blog.DateOfPublishing, blog.AuthorId, blog.Status " +
                    "FROM tblBlogs blog " +
                    "WHERE blog.Id = @id";

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@id", id, DbType.Int32, ParameterDirection.Input);
                blog = db.QuerySingleOrDefault<Blog>(sql, parameter, commandType: CommandType.Text);

                return blog;
            }           
        }

        public Blog RejectBlog(int id)
        {
            Blog blog;
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                string sql = "UPDATE tblBlogs SET Status = 0 " +
                    "WHERE Id = @id " +
                    "SELECT blog.Id, blog.Title, blog.Content, blog.DateOfPublishing, blog.AuthorId, blog.Status " +
                    "FROM tblBlogs blog " +
                    "WHERE blog.Id = @id";

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@id", id, DbType.Int32, ParameterDirection.Input);
                blog = db.QuerySingleOrDefault<Blog>(sql, parameter, commandType: CommandType.Text);

                return blog;
            }
        }

        public Blog Update(Blog updateObject)
        {
            using (IDbConnection db = new SqlConnection(this.connectionString))
            {
                string sql = "UPDATE tblBlogs SET Title = @title, Content = @content, Status = 2 " +
                    "WHERE Id = @id " +
                    "AND Status = 0 " +
                    "SELECT blog.Id, blog.Title, blog.Content, blog.DateOfPublishing, blog.AuthorId, blog.Status " +
                    "FROM tblBlogs blog " +
                    "WHERE blog.Id = @id";

                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@id", updateObject.Id, DbType.Int32, ParameterDirection.Input);
                parameter.Add("@title", updateObject.Title, DbType.AnsiString, ParameterDirection.Input, 70);
                parameter.Add("@content", updateObject.Content, DbType.AnsiString, ParameterDirection.Input, 250);

                updateObject = db.QuerySingleOrDefault<Blog>(sql, parameter, commandType: CommandType.Text);
            }

            return updateObject;
        }
    }
}
