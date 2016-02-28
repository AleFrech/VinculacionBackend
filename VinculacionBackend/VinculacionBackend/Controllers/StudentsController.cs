using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Enums;
using VinculacionBackend.Models;

namespace VinculacionBackend.Controllers
{
   
    public class StudentsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        
        // GET: api/Students
        public IQueryable<User> GetStudents()
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            return db.Users.Where(x=>rels.Any(y=>y.User.Id==x.Id));
        }

        // GET: api/Students/5
        [ResponseType(typeof(User))]
        public IHttpActionResult GetStudent(string id)
        {
            
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.FirstOrDefault(x => rels.Any(y => y.User.Id == x.Id));
            if (student == null )
            {
                return NotFound();
            }
            return Ok(student);
        }

        // PUT: api/Students/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutStudent(string id, User User)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != User.IdNumber)
            {
                return BadRequest();
            }

            var existEmail = EntityExistanceManager.EmailExists(User.Email);
            if (existEmail)
            {
                return InternalServerError(new Exception("Email already exists in database"));
            }

            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var tmpstudent = db.Users.FirstOrDefault(x => rels.Any(y => y.User.Id == x.Id));
            if (tmpstudent != null)
            {
                tmpstudent.ModificationDate = DateTime.Now;
                tmpstudent.Password = User.Password;
                tmpstudent.Name = User.Name;
                tmpstudent.Major = User.Major;
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
        //Put: api/Students/NumberId/Verified
        [ResponseType(typeof(User))]
        [Route("api/Students/{studentsId}/Verified")]
        public IHttpActionResult PutAcceptVerified(string studentsId)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.FirstOrDefault(x => rels.Any(y => y.User.Id == x.Id));
            if (student != null)
            {
                student.Status = Status.Verified;
                db.SaveChanges();
                MailManager.SendSimpleMessage(student.Email, "Fue Aceptado para participar en proyectos de Vinculación",
                    "Vinculación");
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }
        //Get: api/Students/NumberId/Avtive
        [ResponseType(typeof(User))]
        [Route("api/Students/{studentsId}/Active")]
        public IHttpActionResult GetActiveStudent(string studentsId)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student = db.Users.FirstOrDefault(x => rels.Any(y => y.User.Id == x.Id));
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
        //Post: api/Students/NumberId/Rejected
        [ResponseType(typeof(User))]
        [Route("api/Students/{studentsId}/Rejected")]
        public IHttpActionResult PostRejectStudent(string studentsId, RejectedMessage message)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Student");
            var student= db.Users.FirstOrDefault(x => rels.Any(y => y.User.Id == x.Id));
            if (student != null)
            {
                MailManager.SendSimpleMessage(student.Email,message.Message,"Vinculación");
                return Ok(student);
            }
            else
            {
                return NotFound();
            }
        }
        // POST: api/Students
        [ResponseType(typeof(User))]
        public IHttpActionResult PostStudent(User User)
        {
            if (!ModelState.IsValid || User == null)
            {
                return BadRequest(ModelState);
            }

            var existEmail = EntityExistanceManager.EmailExists(User.Email);
            if (existEmail && !MailManager.CheckDomainValidity(User.Email))
            {
                return InternalServerError(new Exception("Email already exists in database"));
            }

            User.Status=Status.Inactive;
            User.CreationDate=DateTime.Now;
            User.ModificationDate=DateTime.Now;
            db.Users.Add(User);
            db.UserRoleRels.Add(new UserRole { User=User,Role=db.Roles.FirstOrDefault(x=>x.Name=="Student")});
            db.SaveChanges();
            
            MailManager.SendSimpleMessage(User.Email,"Hacer click en el siguiente link para Activar: "+ HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority)+"/api/Students/"+ User.IdNumber+"/Active","Vinculación");
            return CreatedAtRoute("DefaultApi", new { id = User.IdNumber }, User);
        }

        // DELETE: api/Students/5
        [ResponseType(typeof(User))]
        public IHttpActionResult DeleteStudent(string id)
        {
            User User = db.Users.FirstOrDefault(x=>x.IdNumber==id);
            User.Status=Status.Inactive;
            if (User == null)
            {
                return NotFound();
            }
            db.SaveChanges();

            return Ok(User);
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
            return db.Users.Count(e => e.IdNumber == id) > 0;
        }





    }

}