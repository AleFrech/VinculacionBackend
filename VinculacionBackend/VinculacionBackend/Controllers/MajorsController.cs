using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Web.Http.Cors;


namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "fiasps.unitec.edu:8085", headers: "*", methods: "*")]
    public class MajorsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // GET: api/Majors
        [Route("api/Majors")]
        public IQueryable<Major> GetMajors()
        {
            return db.Majors;
        }

        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        [Route("api/Majors/{majorId}")]
        public IHttpActionResult GetMajor(string majorId)
        {
            Major major = db.Majors.FirstOrDefault(x => x.MajorId == majorId);
            if (major == null)
            {
                return NotFound();
            }

            return Ok(major);
        }

        // PUT: api/Majors/5
        [ResponseType(typeof(void))]
        [Route("api/Majors/{majorId}")]
        public IHttpActionResult PutMajor(string majorId, Major major)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (majorId != major.MajorId)
            {
                return BadRequest();
            }

            var tmpMajor=db.Majors.FirstOrDefault(x => x.MajorId == majorId);
            tmpMajor.Name = major.Name;
            tmpMajor.MajorId = major.MajorId;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MajorExists(majorId))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(new DbUpdateConcurrencyException());
                }
            }

            return Ok(tmpMajor);
        }

        // POST: api/Majors
        [ResponseType(typeof(Major))]
        [Route("api/Majors")]
        public IHttpActionResult PostMajor([FromBody] Major major)
        {
            if (!ModelState.IsValid || major == null)
            {
                return BadRequest(ModelState);
            }

            //db.Majors.Add(major);
            //db.SaveChanges();

            return Ok(major);
        }

        // DELETE: api/Majors/5
        [Route("api/Majors/{majorId}")]
        [ResponseType(typeof(Major))]
        public IHttpActionResult DeleteMajor(string majorId)
        {
            Major major = db.Majors.FirstOrDefault(x => x.MajorId == majorId);
            if (major == null)
            {
                return NotFound();
            }

           // db.Majors.Remove(major);
           // db.SaveChanges();

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

        private bool MajorExists(string majorId)
        {
            return db.Majors.Count(e => e.MajorId == majorId) > 0;
        }
    }
}