using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Controllers
{
    public class HoursController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // GET: api/Hours
        [Route("api/Hours")]
        public IQueryable<Hour> GetHours()
        {
            return db.Hours.Include(a=>a.User).Include(b=>b.SectionProyect);
        }

        // GET: api/Hours/5
        [ResponseType(typeof(Hour))]
        [Route("api/Hours/{studentId}")]
        public IHttpActionResult GetHour(string studentId)
        {
            Hour hour = db.Hours.Include(x => x.User).Include(b => b.SectionProyect).FirstOrDefault(y => y.User.IdNumber ==studentId);
            if (hour == null)
            {
                return NotFound();
            }

            return Ok(hour);
        }

        // PUT: api/Hours/5
        [ResponseType(typeof(void))]
        [Route("api/Hours")]
        public IHttpActionResult PutHour(HourEntryModel hourModel)
        {

            return InternalServerError(new Exception("Method dosent exist yet"));

            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState);
            //}



            //var tmpHour = db.Hours.FirstOrDefault(x => x.Id == hour.Id);
            //if (tmpHour != null)
            //{
            //    tmpHour.Amount = (hour.Amount >= 0 ? hour.Amount : tmpHour.Amount);
            //    db.Entry(hour).State = EntityState.Modified;


            //    try
            //    {
            //        db.SaveChanges();
            //    }
            //    catch (DbUpdateConcurrencyException)
            //    {
            //        return InternalServerError(new DbUpdateConcurrencyException());
            //    }


            //    return StatusCode(HttpStatusCode.NoContent);
            //}
            //else
            //{
            //    return NotFound();
            //}
        }

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours")]
        public IHttpActionResult PostHour(HourEntryModel hourModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var sectionProjectRel = db.SectionProyectsRels.Include(x => x.Proyect).Include(y => y.Section).FirstOrDefault(z => z.Section.Id == hourModel.SectionId && z.Proyect.Id == hourModel.ProyectId);
            var user = db.Users.FirstOrDefault(x => x.IdNumber == hourModel.NumberId);
            if (user != null && sectionProjectRel != null)
            {
                var hour = new Hour();
                hour.Amount = hourModel.Hour;
                hour.SectionProyect = sectionProjectRel;
                hour.User = user;
                db.Hours.Add(hour);
                db.SaveChanges();
                return Ok(hour);
            }
            else
            {
                return NotFound();
            }
              
        }

        // DELETE: api/Hours/5
        [Route("api/Hours/{HourId}")]
        [ResponseType(typeof(Hour))]
        public IHttpActionResult DeleteHour(long id)
        {
            Hour hour = db.Hours.Find(id);
            if (hour == null)
            {
                return NotFound();
            }

            //db.Hours.Remove(hour);
            db.SaveChanges();
            return Ok(hour);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HourExists(long id)
        {
            return db.Hours.Count(e => e.Id == id) > 0;
        }
    }
}