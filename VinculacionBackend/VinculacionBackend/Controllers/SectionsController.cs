using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Security.Security;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // GET: api/Sections
        [Route("api/Sections")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [EnableQuery]
        public IQueryable<Section> GetSections()
        {
            var sections = db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period);
            return sections;
        }

        // GET: api/Sections/5
        [Route("api/Sections/{sectionId}")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetSection(long sectionId)
        {
            var section = db.Sections.Include(a => a.Class).Include(b => b.User).Include(c => c.Period).FirstOrDefault(d => d.Id == sectionId);
            if (section == null)
            {
                return NotFound();
            }

            return Ok(section);
        }

        // PUT: api/Sections/5
        [ResponseType(typeof(void))]
        [Route("api/Sections/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PutSection(long sectionId, Section section)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (sectionId != section.Id)
            {
                return Unauthorized();
            }

            var tmpSection = db.Sections.FirstOrDefault(x => x.Id == sectionId);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SectionExists(sectionId))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(new DbUpdateConcurrencyException());
                }
            }

            return Ok(tmpSection);
        }

        // POST: api/Sections
        [Route("api/Sections")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostSection(Section section)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Sections.Add(section);
            db.SaveChanges();

            return Ok(section);
        }

        // DELETE: api/Sections/5
        [ResponseType(typeof(Section))]
        [Route("api/Sections/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteSection(long sectionId)
        {
            Section section = db.Sections.Find(sectionId);
            if (section == null)
            {
                return NotFound();
            }

            //db.Sections.Remove(section);
            //db.SaveChanges();

            return Ok(section);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SectionExists(long sectionId)
        {
            return db.Sections.Count(e => e.Id == sectionId) > 0;
        }
    }
}