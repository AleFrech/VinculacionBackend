using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Security;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StudentsController : ApiController
    {
        private readonly StudentsServices _studentsServices = new StudentsServices(new StudentRepository(),new MajorRepository());
        private readonly SendEmail _sendEmail = new SendEmail();
        private readonly Encryption _encryption = new Encryption();


        // GET: api/Students
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [EnableQuery]
        public IQueryable<User> GetStudents()
        {

            return _studentsServices.AllUsers();
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudent(string accountId)
        {
            var student = _studentsServices.Find(accountId);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}/Hour")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudentHour(string accountId)
        {
            var student = _studentsServices.Find(accountId);
            if (student == null)
            {
                return NotFound();
            }

            var total = _studentsServices.GetStudentHours(accountId);
            return Ok(total);
        }

        [Route("api/Students/Filter/{status}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IQueryable<User> GetStudents(string status)
        {
            return _studentsServices.ListbyStatus(status);

        }

        //Put: api/Students/Verified
        [ResponseType(typeof(User))]
        [Route("api/Students/Verified")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutAcceptVerified(VerifiedModel model) 

        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var student = _studentsServices.VerifyUser(model.AccountId);
            if (student != null)
            {
                _sendEmail.Send(student.Email, "Fue Aceptado para participar en Projectos de Vinculación", "Vinculación");
                return Ok(student);
            }

            return NotFound();
        }

        //Get: api/Students/Avtive
        [ResponseType(typeof(User))] 
        [Route("api/Students/{guid}/Active")]
        public IHttpActionResult GetActiveStudent(string guid)
        {

            var accountId = _encryption.Decrypt(HttpContext.Current.Server.UrlDecode(guid));
            var student = _studentsServices.ActivateUser(accountId);
            if (student != null)
            {

                return Ok(student);
            }

            return NotFound();

        }

        //Post: api/Students/Rejected
        [ResponseType(typeof(User))]
        [Route("api/Students/Rejected")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PostRejectStudent(RejectedModel model) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var student = _studentsServices.RejectUser(model.AccountId);
            if (student != null)
            {
                _sendEmail.Send(student.Email, model.Message, "Vinculación");
                return Ok(student);
            }

            return NotFound();
        }
        // POST: api/Students
        [ResponseType(typeof(User))]
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Anonymous")]
        public IHttpActionResult PostStudent(UserEntryModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = _studentsServices.Map(userModel);
            _studentsServices.Add(newUser);
            var stringparameter = _encryption.Encrypt(newUser.AccountId);
            _sendEmail.Send(newUser.Email, 
                "Hacer click en el siguiente link para Activar: " + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)
                + "/api/Students/" + HttpContext.Current.Server.UrlEncode(stringparameter) + "/Active","Vinculación");
            return Ok(newUser);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteStudent(string accountId)
        {

            User user = _studentsServices.DeleteUser(accountId);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }
    }
}