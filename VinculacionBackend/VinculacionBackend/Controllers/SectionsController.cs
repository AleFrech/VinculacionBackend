using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Security.BasicAuthentication;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionsController : ApiController
    {
        private readonly ISectionsServices _sectionServices;

        public SectionsController( ISectionsServices sectionServices)
        {
            _sectionServices = sectionServices;
        }

        // GET: api/Sections
        [Route("api/Sections")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [EnableQuery]
        public IQueryable<Section> GetSections()
        {
            return _sectionServices.All();
        }

        // GET: api/Sections/5
        [Route("api/Sections/{sectionId}")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetSection(long sectionId)
        {
            var section = _sectionServices.Find(sectionId);
            if (section == null)
            {
                return NotFound();
            }

            return Ok(section);
        }

        
        // POST: api/Sections
        [Route("api/Sections")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostSection(Section section)
        {
            
            _sectionServices.Add(section);
            return Ok(section);
        }

        // DELETE: api/Sections/5
        [ResponseType(typeof(Section))]
        [Route("api/Sections/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteSection(long sectionId)
        {
            Section section = _sectionServices.Delete(sectionId);
            if (section == null)
            {
                return NotFound();
            }
            

            return Ok(section);
        }

      
    }
}