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

namespace VinculacionBackend.Controllers
{
    public class MajorsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // GET: api/Majors
        public IQueryable<Major> GetMajors()
        {
            return db.Majors;
        }

        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        public IHttpActionResult GetMajor(string id)
        {
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return NotFound();
            }

            return Ok(major);
        }

        // PUT: api/Majors/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutMajor(string id, Major major)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != major.Id)
            {
                return BadRequest();
            }

            db.Entry(major).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MajorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Majors
        [ResponseType(typeof(Major))]
        public IHttpActionResult PostMajor([FromBody] Major major)
        {
            if (!ModelState.IsValid || major == null)
            {
                return BadRequest(ModelState);
            }

            db.Majors.Add(major);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = major.Id }, major);
        }

        // DELETE: api/Majors/5
        [ResponseType(typeof(Major))]
        public IHttpActionResult DeleteMajor(string id)
        {
            Major major = db.Majors.Find(id);
            if (major == null)
            {
                return NotFound();
            }

            db.Majors.Remove(major);
            db.SaveChanges();

            return Ok(major);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool MajorExists(string id)
        {
            return db.Majors.Count(e => e.Id == id) > 0;
        }
    }
}