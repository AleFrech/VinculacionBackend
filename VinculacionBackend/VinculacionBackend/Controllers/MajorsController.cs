using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using StatsdClient;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MajorsController : ApiController
    {
        private readonly IMajorsServices _majorsServices;
        private readonly IMemoryCacher _memCacher;
        public MajorsController(IMajorsServices majorsServices,IMemoryCacher memcCacher)
        {
            _majorsServices = majorsServices;
            _memCacher = memcCacher;
        }

        // GET: api/Majors
        [Route("api/Majors")]
        [EnableQuery]
        public IEnumerable<Major> GetMajors()
        {
            using (Metrics.StartTimer("majors.get.time"))
            {
                return _memCacher.GetMajors(_majorsServices);
            }
        }
        
        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        [Route("api/Majors/{majorId}")]
        public IHttpActionResult GetMajor(string majorId)
        {
            Major major = _majorsServices.Find(majorId);
            return Ok(major);
        }


        [ResponseType(typeof(Major))]
        [Route("api/Majors/MajorsByProject/{projectId}")]
        public IQueryable<Major> GetMajorsByProject(long projectId)
        {
            return _majorsServices.GetByProject(projectId);
        }
              

    }
}