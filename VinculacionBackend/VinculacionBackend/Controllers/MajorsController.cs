using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.Mvc;
using System.Web.OData;
using System.Web.UI;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;

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
        [System.Web.Http.Route("api/Majors")]
        [EnableQuery]
        public IQueryable<Major> GetMajors()
        {
            return _majorsServices.All();
        }

        // GET: api/Majors/5
        [OutputCache(Duration = 7200, VaryByParam = "none" , Location = OutputCacheLocation.Client, NoStore = false)]
        [ResponseType(typeof(Major))]
        [System.Web.Http.Route("api/Majors/{majorId}")]
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