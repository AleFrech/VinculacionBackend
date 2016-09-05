using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Exceptions;
using VinculacionBackend.Data.Interfaces;
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
                throw new InvalidUsernameOrPasswordException("Usuario o contraseña incorrecto");
            if(user.Status != Status.Active)
                throw new UserNotVerifiedException("El usuario no ha sido verificado aun");
            return user;
        }
        
        public string GetUserRole(string email)
        {
            var role = _userRepository.GetUserRole(email);
            if(role == null)
                throw new NotFoundException("Usuario no Encontrado");
            return role.Name;
        }
    }
}