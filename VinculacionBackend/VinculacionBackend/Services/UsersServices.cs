using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;

namespace VinculacionBackend.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly IUserRepository _userRepository;

        public UsersServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Find(string username, string password)
        {
            var user = _userRepository.GetUserByEmailAndPassword(username, password);
            if(user==null)
                throw new NotFoundException("No se encontro el usuario");
            return user;
        }
    }
}