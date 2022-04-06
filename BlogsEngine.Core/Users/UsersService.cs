using BlogsEngine.Core.Interfaces;
using BlogsEngine.DAL.Interfaces;
using BlogsEngine.Models.Authentication;
using BlogsEngine.Models.Authorization;
using BlogsEngine.Models.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsEngine.Core.Users
{
    public class UsersService : IUsersService
    {
        private IUsersRepository userRepo;
        IJWTUtils jwtUtils;
        private readonly AppSettings appSettings;

        public UsersService(IUsersRepository userRepo, IJWTUtils jwtUtils, IOptions<AppSettings> appSettings)
        {
            this.userRepo = userRepo;
            this.jwtUtils = jwtUtils;
            this.appSettings = appSettings.Value;
        }


        public User Authenticate(JWTValidator auth)
        {
            try
            {
                var user = userRepo.Login(auth.Username);
                return user;
            }
            catch
            {
                throw;
            }
                      
        }

        public User GetUserById(int id)
        {
            var user = userRepo.GetById(id);
            if (user == null) throw new KeyNotFoundException("User not found");
            return user;
        }
    }
}
