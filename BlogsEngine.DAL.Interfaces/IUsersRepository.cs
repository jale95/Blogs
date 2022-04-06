namespace BlogsEngine.DAL.Interfaces
{
    using BlogsEngine.Models.Authentication;
    public interface IUsersRepository: IRepository<User>
    {
        User Login(string username);
    }
}
