using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionProjectsController : ApiController
    {
        private readonly ISectionProjectServices _sectionProjectServices;

        public SectionProjectsController(ISectionProjectServices sectionProjectServices)
        {
            _sectionProjectServices = sectionProjectServices;
        }

        // GET: api/SectionProjects/5
        [ResponseType(typeof(SectionProjectInfoModel))]
        [Route("api/SectionProjects/Info/{sectionprojectId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult GetSectionProject(long sectionprojectId)
        {
            return Ok(_sectionProjectServices.GetInfo(sectionprojectId));
        }

        // PUT: api/SectionProjects/5
        [ResponseType(typeof(void))]
        [Route("api/SectionProjects/Approve/{sectionprojectId}")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutSectionProject(long sectionprojectId)
        {
            _sectionProjectServices.Approve(sectionprojectId);
            return Ok();
        }

    }
}