using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Repositories;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StudentsController : ApiController
    {
        public readonly StudentsServices StudentsServices = new StudentsServices(new StudentRepository(),new MajorRepository());


        // GET: api/Students
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [EnableQuery]
        public IQueryable<User> GetStudents()
        {

            return StudentsServices.AllUsers();
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudent(string accountId)
        {
            var student = StudentsServices.Find(accountId);
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
            var student = StudentsServices.Find(accountId);
            if (student == null)
            {
                return NotFound();
            }

            var total = StudentsServices.GetStudentHours(accountId);
            return Ok(total);
        }

        [Route("api/Students/Filter/{status}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IQueryable<User> GetStudents(string status)
        {
            return StudentsServices.ListbyStatus(status);

        }

        //Put: api/Students/Verified
        [ResponseType(typeof(User))]
        [Route("api/Students/Verified")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutAcceptVerified(VerifiedModel model)  //TODO crear interfas

        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var student = StudentsServices.VerifyUser(model.AccountId);
            if (student != null)
            {
                MailManager.SendSimpleMessage(student.Email, "Fue Aceptado para participar en Projectos de Vinculación", //TODO crear interfaz
                    "Vinculación");
                return Ok(student);
            }

            return NotFound();
        }

        //Get: api/Students/Avtive
        [ResponseType(typeof(User))]  //TODO crear interfaz
        [Route("api/Students/{guid}/Active")]
        public IHttpActionResult GetActiveStudent(string guid)
        {

            var accountId = EncryptDecrypt.Decrypt(HttpContext.Current.Server.UrlDecode(guid));   //TODO Crear interfas
            var student = StudentsServices.ActivateUser(accountId);
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
        public IHttpActionResult PostRejectStudent(RejectedModel model) //TODO crear interefaz
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var student = StudentsServices.RejectUser(model.AccountId);
            if (student != null)
            {
                MailManager.SendSimpleMessage(student.Email, model.Message, "Vinculación");
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

            var newUser = StudentsServices.Map(userModel);
            StudentsServices.Add(newUser);
            var stringparameter = EncryptDecrypt.Encrypt(newUser.AccountId);
            MailManager.SendSimpleMessage(newUser.Email, "Hacer click en el siguiente link para Activar: " + HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + "/api/Students/" + HttpContext.Current.Server.UrlEncode(stringparameter) + "/Active", "Vinculación");
            return Ok(newUser);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteStudent(string accountId)
        {

            User user = StudentsServices.DeleteUser(accountId);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound();
        }
    }
}