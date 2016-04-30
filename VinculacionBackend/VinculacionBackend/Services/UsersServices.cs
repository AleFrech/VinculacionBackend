using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
namespace VinculacionBackend.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly IUserRepository _userRepository;

        public UsersServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Find(string user, string password)
        {
           return _userRepository.GetUserByEmailAndPassword(user, password);
        }
    }
}