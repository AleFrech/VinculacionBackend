using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Enums;
using VinculacionBackend.Models;
using System.Web.Http.Cors;


namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class LoginController : ApiController
    {

        private VinculacionContext db = new VinculacionContext();


        [Route("api/Login")]
        [ResponseType(typeof (User))]
        [CustomAuthorize(Roles = "Anonymous")]
        public IHttpActionResult PostUserLogin(LoginUserModel loginUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.Email.Contains(loginUser.User) && u.Password == loginUser.Password);

          
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