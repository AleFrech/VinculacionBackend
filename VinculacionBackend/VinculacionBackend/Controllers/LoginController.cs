using System.Web.Http;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Security.BasicAuthentication;


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
        [ValidateModel]
        public IHttpActionResult PostUserLogin(LoginUserModel loginUser)
        {
            var user = _usersServices.FindValidUser(loginUser.User, _encryption.Encrypt(loginUser.Password));
            string userInfo = user.Email + ":" + user.Password;
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(userInfo);
            string token = System.Convert.ToBase64String(plainTextBytes);
            return Ok("Basic " + token);

        }

        [Route("api/Login/GetUserRole")]
        [ValidateModel]
        public string PostUserRole(EmailModel model)
        {
            return _usersServices.GetUserRole(model.Email);
        }
    }
}