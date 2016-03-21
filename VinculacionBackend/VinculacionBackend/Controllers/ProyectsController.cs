using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Models;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectsController : ApiController
    {
        private ProjectSevices _services = new ProjectServices();

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
            Project Project = _services.Find(projectId);
            if (Project == null)
            {
                return NotFound();
            }

            return Ok(Project);
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
        public IHttpActionResult PutProject(long projectId, ProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tmpProject = db.Projects.FirstOrDefault(x => x.Id == projectId);
            if (tmpProject != null)
            {
                tmpProject.Name = model.Name;
                tmpProject.Description = model.Description;
            }
            else
            {
                return NotFound();
            }
            
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(projectId))
                {
                    return NotFound();
                }
                else
                {
                    return InternalServerError(new DbUpdateConcurrencyException());
                }
            }

            return Ok(tmpProject);
        }

        // POST: api/Projects
        [Route("api/Projects")]
        [ResponseType(typeof(Project))]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostProject(ProjectModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var project= new Project();
            project.Name = model.Name;
            project.Description = model.Description;
            _services.Add(project);

            return Ok(project);
        }

        // DELETE: api/Projects/5
        [Route("api/Projects/{projectId}")]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(long projectId)
        {
            Project project = _services.Delete(projectId);
            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProjectExists(long id)
        {
            return db.Projects.Count(e => e.Id == id) > 0;
        }
    }
}