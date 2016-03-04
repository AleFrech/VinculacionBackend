using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
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
        public IHttpActionResult PostUserLogin(LoginUserModel loginUser)
        {
            if (!ModelState.IsValid || loginUser == null)
            {
                return BadRequest(ModelState);
            }

            var user = db.Users.FirstOrDefault(u => u.Email == loginUser.User && u.Password == loginUser.Password);
            if (user != null /*&& user.Status!= Status.Inactive*/)
            {
                return Ok(user);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}