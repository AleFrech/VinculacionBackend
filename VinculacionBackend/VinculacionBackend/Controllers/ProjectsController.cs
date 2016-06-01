using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using Novacode;
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
            var currentUser = (CustomPrincipal)HttpContext.Current.User;
            return _services.GetUserProjects(currentUser.UserId, currentUser.roles);
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

        // GET: api/Projects/FinalReport/
        [Route("api/Projects/FinalReport")]
        public HttpResponseMessage GetProjectFinalReport()
        {

            var fileName = @"test.docx";
            var doc = DocX.Create(fileName);
            doc.InsertParagraph("This is my first paragraph");
            var ms = new MemoryStream();
            doc.SaveAs(ms);
            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(ms) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return response;
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