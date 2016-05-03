using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectsController : ApiController
    {
        private readonly IProjectServices _services;

        public ProjectsController(IProjectServices services)
        {
            _services = services;
        }


        // GET: api/Projects
        [Route("api/Projects")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Project> GetProjects()
        {
            return _services.All();
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("api/Projects/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetProject(long projectId)
        {
            Project project = _services.Find(projectId);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("api/Projects/Students/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<User> GetProjectStudents(long projectId)
        {
            return _services.GetProjectStudents(projectId);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        [Route("api/Projects/{projectId}")]
        [ValidateModel]
        public IHttpActionResult PutProject(long projectId, ProjectModel model)
        {
            var tmpProject = _services.UpdateProject(projectId, model);

            if (tmpProject == null)
            {
                return NotFound();
            }

            return Ok(tmpProject);
        }

        // POST: api/Projects
        [Route("api/Projects")]
        [ResponseType(typeof(Project))]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostProject(ProjectModel model)
        {

            var project = _services.Add(model);
            return Ok(project);

        }

        // DELETE: api/Projects/5
        //[Route("api/Projects/{projectId}")]
        //[CustomAuthorize(Roles = "Admin")]
        //[ResponseType(typeof(Project))]
        //public IHttpActionResult DeleteProject(long projectId)
        //{
        //    Project project = _services.Delete(projectId);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(project);
        //}
    }
}