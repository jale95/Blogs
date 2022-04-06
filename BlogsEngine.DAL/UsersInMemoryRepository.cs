using BlogsEngine.DAL.Interfaces;
using BlogsEngine.Models.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogsEngine.DAL
{
    public  class UsersInMemoryRepository : IUsersRepository
    {
        private static readonly List<User> Users = new User[]
            {
                new User
                {
                    Id = 1,
                    UserName = "JorgeEditor",
                    Role = Role.Editor,
                },
                new User
                {
                    Id = 2,
                    UserName = "JorgeWriter",
                    Role = Role.Writer,
                },
                new User
                {
                    Id = 3,
                    UserName = "JorgePublic",
                    Role = Role.Public,
                }               
            }.ToList();

        public User Add(User newobject)
        {
            throw new NotImplementedException();
        }

        public User Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User GetById(int id)
        {
            throw new NotImplementedException();
        }

        public User Login(string username)
        {
            var user = Users.FirstOrDefault(x => x.UserName == username);

            if (user == null)
            {
                throw new Exception("Username or password is incorrect");
            }

            return user;

        }

        public User Update(User updateObject)
        {
            throw new NotImplementedException();
        }

        //SIMULATING ENCODING
        private string hashPassword(string password)
        {
            switch (password)
            {
                case "password1":
                    return "qwerty";
                case "password2":
                    return "asdfgh";
                case "password3":
                    return "zxcvbn";
                default:
                    return "";
            }
        }
    }
}
