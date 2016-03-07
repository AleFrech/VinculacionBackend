using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Enums;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using WebGrease.Css.Extensions;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class StudentsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();


        // GET: api/Students
        [Route("api/Students")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IQueryable<User> GetStudents()
        {
            var u = HttpContext.Current.User.Identity;
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            return db.Users.Include(m =>m.Major).Where(x => rels.Any(y => y.User.Id == x.Id));
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetStudent(string accountId)
        {  
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountId);
            if (User == null)
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
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountId);
            if (student == null)
            {
                return NotFound();
            }           
            var hour = db.Hours.Include(a => a.User).Where(x => x.User.Id == student.Id);
            hour.ForEach(x =>
            {
                total += x.Amount;
            });
            return Ok(total);
        }

        [Route("api/Students/Filter/{status}")]
        [CustomAuthorize(Roles = "Admin,Professor")]

        public IQueryable<User> GetStudents(string status)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
         
            if(status=="Inactive")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Inactive);
            if (status == "Active")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Active);
            if (status == "Verified")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Verified);
            if (status == "Rejected")
                return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id) && x.Status == Status.Rejected);
            return null;
        }

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        [Route("api/Students/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult PutStudent(string accountId, UserEntryModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (accountId != userModel.AccountId)
            {
                return Unauthorized();
            }

            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var tmpstudent = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountId);
            if (tmpstudent != null)
            {
                tmpstudent.ModificationDate = DateTime.Now;
                tmpstudent.Password = userModel.Password;
                tmpstudent.Name = userModel.Name;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {  
                    return InternalServerError(new DbUpdateConcurrencyException());
                }
                return Ok(tmpstudent);
            }
            return NotFound();
        }

        //Put: api/Students/Verified
        [ResponseType(typeof(User))]
        [Route("api/Students/Verified")]
         [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutAcceptVerified(VerifiedModel model)

        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == model.AccountId);
            if (student != null)
            {
                student.Status = Status.Verified;
                db.SaveChanges();
                MailManager.SendSimpleMessage(student.Email, "Fue Aceptado para participar en Projectos de Vinculación",
                    "Vinculación");
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }

        //Get: api/Students/Avtive
        [ResponseType(typeof(User))]
        [Route("api/Students/{guid}/Active")]
        public IHttpActionResult GetActiveStudent(string guid)

        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var accountId = EncryptDecrypt.Decrypt(HttpContext.Current.Server.UrlDecode(guid));
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountId);
            if (student != null)
            {
                student.Status = Status.Active;
                db.SaveChanges();
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }

        //Post: api/Students/Rejected
        [ResponseType(typeof(User))]
        [Route("api/Students/Rejected")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PostRejectStudent(RejectedModel model)
        {
            if (!ModelState.IsValid || model == null)
            {
                return BadRequest();
            }

            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student= db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == model.AccountId);
            if (student != null)
            {
                MailManager.SendSimpleMessage(student.Email,model.Message,"Vinculación");
                student.Status=Status.Rejected;
                db.SaveChanges();
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
                return InternalServerError(new Exception("A specific field was null"));
            }

            if (EntityExistanceManager.EmailExists(userModel.Email))
            {
                return InternalServerError(new Exception("Email already exists in database"));
            }
            if (!MailManager.CheckDomainValidity(userModel.Email))
            {
                return InternalServerError(new Exception("Email domain is invalid"));
            }

            var major= db.Majors.FirstOrDefault(x => x.MajorId == userModel.MajorId);

            if (major == null)
            {
                return InternalServerError(new Exception("Please Enter a valid Major Id"));
            }

            var newUser=new User();
            newUser.AccountId = userModel.AccountId;
            newUser.Name = userModel.Name;
            newUser.Password = userModel.Password;
            newUser.Major = major;
            newUser.Campus = userModel.Campus;
            newUser.Email = userModel.Email;
            newUser.Status=Status.Inactive;
            newUser.CreationDate=DateTime.Now;
            newUser.ModificationDate=DateTime.Now;
            db.Users.Add(newUser);
            db.UserRoleRels.Add(new UserRole { User=newUser,Role=db.Roles.FirstOrDefault(x=>x.Name=="Student")});
            db.SaveChanges();
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool StudentExists(string id)
        {
            return db.Users.Count(e => e.AccountId == id) > 0;
        }

        private bool CheckUserModel(UserEntryModel model)
        {
            bool isvalid = (model.MajorId != null) && (model.AccountId != null) && (model.Campus != null) && (model.Email != null) &&
                (model.Name != null) && (model.Password != null);
            return isvalid;
        }
    }
}