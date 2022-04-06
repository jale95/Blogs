namespace BlogsEngine.Core.Interfaces
{
    using BlogsEngine.Models.Authentication;
    using BlogsEngine.Models.Authorization;

    public interface IUsersService
    {
        User Authenticate(JWTValidator auth); 
        User GetUserById(int id);
    }
}
