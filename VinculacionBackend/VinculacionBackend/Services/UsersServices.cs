using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Services
{
    public class UsersServices : IUsersServices
    {
        private readonly IUserRepository _userRepository;

        public UsersServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User FindValidUser(string username, string password)
        {
            var user = _userRepository.GetUserByEmailAndPassword(username, password);
            if (user == null  )
                throw new NotFoundException("Usuario o contraseña incorrecto");
            if(user.Status != Status.Verified)
                throw new NotFoundException("El usuario no ha sido verificado aun");
            return user;
        }
    }
}