using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Entities;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Repositories;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MajorsController : ApiController
    {
        private readonly IMajorsServices _majorsServices;

        public MajorsController(IMajorsServices majorsServices)
        {
            _majorsServices = majorsServices;
        }

        // GET: api/Majors
        [Route("api/Majors")]
        [CustomAuthorize(Roles = "Anonymous")]
        [EnableQuery]
        public IQueryable<Major> GetMajors()
        {
            return _majorsServices.All();
        }

        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        [Route("api/Majors/{majorId}")]
        [CustomAuthorize(Roles = "Anonymous")]
        public IHttpActionResult GetMajor(string majorId)
        {
            Major major = _majorsServices.Find(majorId);
            if (major == null)
            {
                return NotFound();
            }

            return Ok(major);
        }

       
    }
}