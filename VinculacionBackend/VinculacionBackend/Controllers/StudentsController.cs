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
        public IQueryable<User> GetStudents()
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id));
            //return db.Users;
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
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
        public IQueryable<User> GetStudents(int status)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            return db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id )&& (int)x.Status==status);
            //return db.Users;
        }


        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        [Route("api/Students/{accountId}")]
        public IHttpActionResult PutStudent(string accountId, UserEntryModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (accountId != userModel.AccountId)
            {
                return BadRequest();
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


        [CustomAuthorize(Roles = "Admin")]
        //Put: api/Students/accountId/Verified
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}/Verified")]
        public IHttpActionResult PutAcceptVerified(string accountId)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountId);
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
        //Get: api/Students/accountId/Avtive
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}/Active")]
        public IHttpActionResult GetActiveStudent(string accountId)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
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



        [CustomAuthorize(Roles = "Admin")]
        //Post: api/Students/accountId/Rejected
        [ResponseType(typeof(User))]
        [Route("api/Students/Rejected")]
        public IHttpActionResult PostRejectStudent(RejectedModel model)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student= db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == model.AccountId);
            if (student != null)
            {
                MailManager.SendSimpleMessage(student.Email,model.Message,"Vinculación");
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
        public IHttpActionResult PostStudent(UserEntryModel userModel)
        {
            if (!ModelState.IsValid || userModel == null)
            {
                return BadRequest(ModelState);
            }
            var existEmail = EntityExistanceManager.EmailExists(userModel.Email);
            if (existEmail || !MailManager.CheckDomainValidity(userModel.Email))
            {
                return InternalServerError(new Exception("Email already exists in database"));
            }
            var newUser=new User();
            newUser.AccountId = userModel.AccountId;
            newUser.Name = userModel.Name;
            newUser.Password = userModel.Password;
            newUser.Major = db.Majors.FirstOrDefault(x => x.MajorId == userModel.MajorId);
            newUser.Campus = userModel.Campus;
            newUser.Email = userModel.Email;
            newUser.Status=Status.Inactive;
            newUser.CreationDate=DateTime.Now;
            newUser.ModificationDate=DateTime.Now;
            db.Users.Add(newUser);
            db.UserRoleRels.Add(new UserRole { User=newUser,Role=db.Roles.FirstOrDefault(x=>x.Name=="Student")});
            db.SaveChanges();
            MailManager.SendSimpleMessage(newUser.Email,"Hacer click en el siguiente link para Activar: "+ HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)+"/api/Students/"+ newUser.AccountId+"/Active","Vinculación");
            return Ok(newUser);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(User))]
        [Route("api/Students/{accountId}")]
        public IHttpActionResult DeleteStudent(string id)
        {
            
            User User = db.Users.FirstOrDefault(x=>x.AccountId==id);
            if (User != null)
            {
                User.Status = Status.Inactive;
               // db.Users.Remove(User);
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
    }

}