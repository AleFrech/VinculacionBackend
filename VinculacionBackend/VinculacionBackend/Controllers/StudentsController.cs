using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Enums;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.Services;
using WebGrease.Css.Extensions;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StudentsController : ApiController
    {
        readonly StudentsServices _studentsServices = new StudentsServices();


        // GET: api/Students
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Admin,Professor")]
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
            var total = 0;
            var student = _studentsServices.Find(accountId);
            if (student == null)
            {
                return NotFound();
            }

            total = _studentsServices.StudentHours(accountId);
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
        public IHttpActionResult PutAcceptVerified(VerifiedModel model)  //TODO crear interfas

        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }

            if (model.AccountId == null)
            {

                return InternalServerError(new Exception("El numero de cuenta estas vacio"));
            }


            var student = _studentsServices.Find(model.AccountId);
            if (student != null)
            {
                MailManager.SendSimpleMessage(student.Email, "Fue Aceptado para participar en Projectos de Vinculación", //TODO crear interfaz
                    "Vinculación");
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }

        //Get: api/Students/Avtive
        [ResponseType(typeof(User))]  //TODO crear interfaz
        [Route("api/Students/{guid}/Active")]
        public IHttpActionResult GetActiveStudent(string guid)

        {
           
            var accountId = EncryptDecrypt.Decrypt(HttpContext.Current.Server.UrlDecode(guid));   //TODO Crear interfas
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
        public IHttpActionResult PostRejectStudent(RejectedModel model) //TODO crear interefaz
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }
            if (model.AccountId == null || model.Message == null)
            {
                return InternalServerError(new Exception("Uno o mas campos vacios"));
            }
            var student = _studentsServices.RejectUser(model.AccountId);
            if (student!=null)
            {
                MailManager.SendSimpleMessage(student.Email, model.Message, "Vinculación");
                return Ok(student);
            }
            else
            {
                return NotFound();

            }
        }
        // POST: api/Students
        [ResponseType(typeof(User))]
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Anonymous")]
        public IHttpActionResult PostStudent(UserEntryModel userModel)
        {

            if (!ModelState.IsValid || userModel == null)
            {
                return BadRequest(ModelState);
            }

            if (!CheckUserModel(userModel))
            {
                return InternalServerError(new Exception("Uno o mas campos vacios"));
            }

            if (EntityExistanceManager.EmailExists(userModel.Email))
            {
                return InternalServerError(new Exception("El correo ya existe"));
            }
            if (EntityExistanceManager.AccountNumberExists(userModel.AccountId))
            {
                return InternalServerError(new Exception("El numbero de cuenta ya existe"));
            }
            if (!MailManager.CheckDomainValidity(userModel.Email))
            {
                return InternalServerError(new Exception("Correo no valido"));
            }
            var newUser = new User();
            _studentsServices.Map(newUser, userModel);
            _studentsServices.Add(newUser);
            var stringparameter = EncryptDecrypt.Encrypt(newUser.AccountId);
            MailManager.SendSimpleMessage(newUser.Email,"Hacer click en el siguiente link para Activar: "+ HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)+"/api/Students/"+HttpContext.Current.Server.UrlEncode(stringparameter)+"/Active","Vinculación");
            return Ok(newUser);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteStudent(string accountId)
        {            
            User User = db.Users.FirstOrDefault(x=>x.AccountId==accountId);
            if (User != null)
            {
                var userrole =db.UserRoleRels.FirstOrDefault(x => x.User.AccountId == User.AccountId);
                db.UserRoleRels.Remove(userrole);
                db.Users.Remove(User);
                db.SaveChanges();
                return Ok(User);
            }
            else
            {
                return NotFound();
            } 
        }

     
    }
}