using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

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
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Section> GetSections()
        {
            var currentUser = (CustomPrincipal)HttpContext.Current.User;
            return _sectionServices.AllByUser(currentUser.UserId, currentUser.roles);
        }


        // GET: api/Sections
        [Route("api/Sections/CurrentPeriodSections")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Section> GetCurrentPeriodSections()
        {
            var currentUser = (CustomPrincipal)HttpContext.Current.User;
            return _sectionServices.GetCurrentPeriodSectionsByUser(currentUser.UserId, currentUser.roles.Single());
        }


        // GET: api/Sections
        [Route("api/Sections/SectionsByProject/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Section> GetSectionsByProject(long projectId)
        {
            var currentUser = (CustomPrincipal)HttpContext.Current.User;
            var sections = _sectionServices.GetSectionsByProject(projectId,currentUser.roles.Single(),currentUser.UserId);
            return sections;
        }

        // GET: api/Sections/5
        [Route("api/Sections/{sectionId}")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetSection(long sectionId)
        {
            var section = _sectionServices.Find(sectionId);
            return Ok(section);
        }

        // GET: api/Sections/5
        [ResponseType(typeof(Project))]
        [Route("api/Sections/Students/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<User> GetSectionStudents(long sectionId)
        {
            return _sectionServices.GetSectionStudents(sectionId);
        }

        [ResponseType(typeof(Project))]
        [Route("api/Sections/StudentsHour/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<object> GetSectionStudentsHour(long sectionId)
        {
            return _sectionServices.GetSectionStudentsHour(sectionId);
        }

        // GET: api/Sections/5
        [ResponseType(typeof(Project))]
        [Route("api/Sections/Projects/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<Project> GetSectionProjects(long sectionId)
        {
            return _sectionServices.GetSectionsProjects(sectionId);
        }


        // POST: api/Sections
        [Route("api/Sections")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostSection(SectionEntryModel sectionModel)
        {
            var section = new Section();
            _sectionServices.Map(section,sectionModel);
            _sectionServices.Add(section);
            return Ok(section);
        }

        [Route("api/Sections/AssignStudents")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostAssignStudents(SectionStudentModel model)
        {

            _sectionServices.AssignStudents(model);
            return Ok();
        }
        
        [Route("api/Sections/RemoveStudents")]
        [ResponseType(typeof(Section))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        [ValidateModel]
        public IHttpActionResult PostRemoveStudents(SectionStudentModel model)
        {

            _sectionServices.RemoveStudents(model);
            return Ok();
        }

        // PUT: api/Sections/5
        [ResponseType(typeof(Section))]
        [Route("api/Sections/{sectionId}")]
        [ValidateModel]
        public IHttpActionResult PutSection(long sectionId,SectionEntryModel model)
        {

            var tmpSection = _sectionServices.UpdateSection(sectionId, model);
            return Ok(tmpSection);
        }

        // DELETE: api/Sections/5
        [ResponseType(typeof(Section))]
        [Route("api/Sections/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult DeleteSection(long sectionId)
        {
            Section section = _sectionServices.Delete(sectionId);
            return Ok(section);
        }

      
    }
}