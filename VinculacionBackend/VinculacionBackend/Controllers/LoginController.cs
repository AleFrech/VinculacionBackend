using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using System.Web.UI;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Security;
using VinculacionBackend.Security.BasicAuthentication;
using VinculacionBackend.Services;


namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private readonly IUsersServices _usersServices;
        private readonly IEncryption _encryption;

        public LoginController(IUsersServices usersServices, IEncryption encryption)
        {
            _usersServices = usersServices;
            _encryption = encryption;
        }

        [Route("api/Login")]
        [ResponseType(typeof (User))]
        [CustomAuthorize(Roles = "Anonymous")]
        [ValidateModel]
        public IHttpActionResult PostUserLogin(LoginUserModel loginUser)
        {
            var passw = _encryption.Encrypt(loginUser.Password);
            var user = _usersServices.Find(loginUser.User, _encryption.Encrypt(loginUser.Password));

          
                if (user != null)
                {
                   
                    if (!user.Email.Equals(loginUser.User) || !user.Password.Equals(_encryption.Encrypt(loginUser.Password)) || user.Status!= Status.Verified)
                    {
                     throw new UnauthorizedException("Usuario o contraseña incorrecto");
                    }

                    string userInfo = user.Email + ":" + user.Password;
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(userInfo);
                    string token = System.Convert.ToBase64String(plainTextBytes);

                    return Ok("Basic " + token);
                }
            throw new UnauthorizedException("Usuario o contraseña incorrecto");

        }
    }
}