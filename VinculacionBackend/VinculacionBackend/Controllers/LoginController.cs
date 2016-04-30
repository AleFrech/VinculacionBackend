using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using System.Web.UI;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Security.BasicAuthentication;
using VinculacionBackend.Services;


namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {
        private readonly IUsersServices _usersServices;

        public LoginController(IUsersServices usersServices)
        {
            _usersServices = usersServices;
        }

        [Route("api/Login")]
        [ResponseType(typeof (User))]
        [CustomAuthorize(Roles = "Anonymous")]
        public IHttpActionResult PostUserLogin(LoginUserModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _usersServices.Find(loginUser.User, loginUser.Password);

          
                if (user != null)
                {
                   
                    if (!user.Email.Equals(loginUser.User) || !user.Password.Equals(loginUser.Password) || user.Status!= Status.Verified)
                    {
                    return Unauthorized();
                    }

                    string userInfo = user.Email + ":" + user.Password;
                    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(userInfo);
                    string token = System.Convert.ToBase64String(plainTextBytes);

                    return Ok("Basic " + token);
                }
                else
                {
                    return Unauthorized();
                }
            
        }
    }
}