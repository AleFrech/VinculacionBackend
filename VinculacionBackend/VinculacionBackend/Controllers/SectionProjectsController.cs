using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
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


        // GET: api/SectionProjects
        [Route("api/SectionProjects/UnApproved/")]
        [CustomAuthorize(Roles = "Admin")]
        public IQueryable<SectionProject> GetSectionProjecstUnApproved()
        {
            return _sectionProjectServices.GetUnapproved();
        }


        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects/Info/{sectionprojectId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public SectionProject GetSectionProject(long sectionprojectId)
        {
            return _sectionProjectServices.GetInfo(sectionprojectId);
        }

        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects/Info/{sectionId}/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public SectionProject GetSectionProject(long sectionId,long projectId)
        {
            return _sectionProjectServices.GetInfo(sectionId, projectId);
        }

        // POST: api/SectionProjects
        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostSectionProject(SectionProjectEntryModel sectionProjectEntryModel)
        {
            
            return Ok(_sectionProjectServices.AddOrUpdate(sectionProjectEntryModel));
        }

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