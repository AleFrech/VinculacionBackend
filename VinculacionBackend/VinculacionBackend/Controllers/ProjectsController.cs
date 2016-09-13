using System;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

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



        // GET: api/Projects
        [Route("api/Projects/ProjectsByUser")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Project> GetProjectsByUser()
        {
            var currentUser = (CustomPrincipal)HttpContext.Current.User;
            return _services.GetUserProjects(currentUser.UserId, currentUser.roles);
        }

        [Route("api/ProjectsCount")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public int GetProjectsCount()
        {
            var currentUser = (CustomPrincipal)HttpContext.Current.User;
            return _services.GetUserProjects(currentUser.UserId, currentUser.roles).Count();
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("api/Projects/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IHttpActionResult GetProject(long projectId)
        {
            Project project = _services.Find(projectId);
            return Ok(project);
        }

        // Get: api/Projects/FinalReport/
        [Route("api/Projects/FinalReport/{projectId}/{sectionId}/{fieldHours}/{calification}/{beneficiariesQuantities}/{beneficiariGroups}")]
        public HttpResponseMessage GetProjectFinalReport(long projectId,long sectionId,int fieldHours, int calification, int beneficiariesQuantities, string beneficiariGroups)
        {
            return _services.GetFinalReport(projectId,sectionId,fieldHours,calification,beneficiariesQuantities,beneficiariGroups);
        }

        // GET: api/Projects/5
        [ResponseType(typeof(Project))]
        [Route("api/Projects/Students/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        public IQueryable<User> GetProjectStudents(long projectId)
        {
            return _services.GetProjectStudents(projectId);
        }


        // GET: api/Projects
        [Route("api/Projects/ProjectsBySection/{sectionId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        public IQueryable<Project> GetProjectsBySection(long sectionId)
        {
            var projects = _services.GetProjectsBySection(sectionId);
            return projects;
        }

        // PUT: api/Projects/5
        [ResponseType(typeof(void))]
        [Route("api/Projects/{projectId}")]
        [ValidateModel]
        public IHttpActionResult PutProject(long projectId, ProjectModel model)
        {
            var tmpProject = _services.UpdateProject(projectId, model);
            return Ok(tmpProject);
        }

        [ResponseType(typeof(void))]
        [Route("api/Projects/AssignSection")]
        [ValidateModel]
        public IHttpActionResult PostAssignSection(ProjectSectionModel model)
        {
            _services.AssignSection(model);
            return Ok();
        }

        [ResponseType(typeof(void))]
        [Route("api/Projects/AssignProjectsToSection")]
        [ValidateModel]
        public IHttpActionResult PostAssignSections(ProjectsSectionModel model)
        {
            _services.AssignProjectsToSection(model);
            return Ok();
        }


        [ResponseType(typeof(void))]
        [Route("api/Projects/RemoveSection")]
        [ValidateModel]
        public IHttpActionResult PostRemoveSection(ProjectSectionModel model)
        {
             _services.RemoveFromSection(model.ProjectId,model.SectionId);
            return Ok();
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

        //DELETE: api/Projects/5
        [Route("api/Projects/{projectId}")]
        [CustomAuthorize(Roles = "Admin")]
        [ResponseType(typeof(Project))]
        public IHttpActionResult DeleteProject(long projectId)
        {
            Project project = _services.Delete(projectId);
            return Ok(project);
        }
    }
}