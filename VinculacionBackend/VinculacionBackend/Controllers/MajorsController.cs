using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Cache;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MajorsController : ApiController
    {
        private readonly IMajorsServices _majorsServices;
        private readonly MemoryCacher _memCacher;
        public MajorsController(IMajorsServices majorsServices)
        {
            _majorsServices = majorsServices;
            _memCacher = new MemoryCacher();
        }

        // GET: api/Majors
        [Route("api/Majors")]
        [EnableQuery]
        public IEnumerable<Major> GetMajors()
        {
            var result = _memCacher.GetValue("majors");
            if (result == null)
            {
                _memCacher.Add("majors", _majorsServices.All().ToList(), DateTimeOffset.UtcNow.AddHours(24));
                result = _memCacher.GetValue("majors");
            }
            return result as IEnumerable<Major>;        
        }
      
    

        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        [Route("api/Majors/{majorId}")]
        public IHttpActionResult GetMajor(string majorId)
        {
            Major major = _majorsServices.Find(majorId);
            return Ok(major);
        }   
    }
}