using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ClassesController : ApiController
    {
        private readonly IClassesServices _classesServices;

        public ClassesController(IClassesServices classesServices)
        {
            _classesServices = classesServices;
        }

        // GET: api/Classes
        [Route("api/Classes")]
        [EnableQuery]
        public IQueryable<Class> GetClasses()
        {
            return _classesServices.All();
        }

        // GET: api/Classes/5
        [ResponseType(typeof(Class))]
        [Route("api/Classes/{id}")]
        public IHttpActionResult GetClass(long id)
        {
            Class @class = _classesServices.Find(id);
            return Ok(@class);
        }

        // POST: api/Classes
        [ResponseType(typeof(Class))]
        [Route("api/Classes")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostClass(ClassEntryModel classModel)
        {
            var newClass = new Class();
            _classesServices.Map(newClass,classModel);
            _classesServices.Add(newClass);
            return Ok(newClass);
        }


        [ResponseType(typeof(Class))]
        [Route("api/Classes/{classId}")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PutClass(long classId,ClassEntryModel classModel)
        {
            var Class = _classesServices.UpdateClass(classId, classModel);
            return Ok(Class);
        }

        // DELETE: api/Classes/5
        [Route("api/Classes/{id}")]
        [ResponseType(typeof(Class))]
        public IHttpActionResult DeleteClass(long id)
        {
            var @class = _classesServices.Delete(id);
            return Ok(@class);
        }
    }
}