using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Web.Http.Cors;
using System.Web.OData;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MajorsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // GET: api/Majors
        [Route("api/Majors")]
        [CustomAuthorize(Roles = "Anonymous")]
        [EnableQuery]
        public IQueryable<Major> GetMajors()
        {
            return db.Majors;
        }

        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        [Route("api/Majors/{majorId}")]
        [CustomAuthorize(Roles = "Anonymous")]
        public IHttpActionResult GetMajor(string majorId)
        {
            Major major = db.Majors.FirstOrDefault(x => x.MajorId == majorId);
            if (major == null)
            {
                return NotFound();
            }

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