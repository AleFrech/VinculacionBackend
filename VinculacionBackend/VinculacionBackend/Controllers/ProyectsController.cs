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
    public class ProyectsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // GET: api/Proyects
        [Route("api/Proyects")]
        public IQueryable<Proyect> GetProyects()
        {
            return db.Proyects;
        }

        // GET: api/Proyects/5
        [ResponseType(typeof(Proyect))]
        [Route("api/Proyects/{proyectId}")]
        public IHttpActionResult GetProyect(long proyectId)
        {
            Proyect proyect = db.Proyects.Find(proyectId);
            if (proyect == null)
            {
                return NotFound();
            }

            return Ok(proyect);
        }

        // PUT: api/Proyects/5
        [ResponseType(typeof(void))]
        [Route("api/Proyects/{proyectId}")]
        public IHttpActionResult PutProyect(long proyectId, Proyect proyect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (proyectId != proyect.Id)
            {
                return BadRequest();
            }

            var tmpProyect = db.Proyects.FirstOrDefault(x => x.Id == proyectId);
            tmpProyect.Name = proyect.Name;
            tmpProyect.Description = proyect.Description;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProyectExists(proyectId))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(new DbUpdateConcurrencyException());
                }
            }

            return Ok(tmpProyect);
        }

        // POST: api/Proyects
        [Route("api/Proyects")]
        [ResponseType(typeof(Proyect))]
        public IHttpActionResult PostProyect(Proyect proyect)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Proyects.Add(proyect);
            db.SaveChanges();

            return Ok(proyect);
        }

        // DELETE: api/Proyects/5
        [Route("api/Proyects/{proyectId}")]
        [ResponseType(typeof(Proyect))]
        public IHttpActionResult DeleteProyect(long proyectId)
        {
            Proyect proyect = db.Proyects.Find(proyectId);
            if (proyect == null)
            {
                return NotFound();
            }

            //db.Proyects.Remove(proyect);
            //db.SaveChanges();

            return Ok(proyect);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProyectExists(long id)
        {
            return db.Proyects.Count(e => e.Id == id) > 0;
        }
    }
}