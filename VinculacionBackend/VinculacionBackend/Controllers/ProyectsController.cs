using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using System.Web.Http.Cors;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProjectsController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // GET: api/Projects
        [Route("api/Projects")]
        public IQueryable<Project> GetProjects()
        {
            return db.Projects;
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("api/Projects/{ProjectId}")]
        public IHttpActionResult GetProject(long ProjectId)
        {
            Project Project = db.Projects.Find(ProjectId);
            if (Project == null)
            {
                return NotFound();
            }

            return Ok(Project);
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        [Route("api/Projects/{ProjectId}")]
        public IHttpActionResult PutProject(long ProjectId, Project Project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (ProjectId != Project.Id)
            {
                return BadRequest();
            }

            var tmpProject = db.Projects.FirstOrDefault(x => x.Id == ProjectId);
            tmpProject.Name = Project.Name;
            tmpProject.Description = Project.Description;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(ProjectId))
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
        public IHttpActionResult PostProject(Project Project)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Projects.Add(Project);
            db.SaveChanges();

            return Ok(Project);
        }

        // DELETE: api/Projects/5
        [Route("api/Projects/{ProjectId}")]
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(long ProjectId)
        {
            Project Project = db.Projects.Find(ProjectId);
            if (Project == null)
            {
                return NotFound();
            }

            //db.Projects.Remove(Project);
            //db.SaveChanges();

            return Ok(Project);
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