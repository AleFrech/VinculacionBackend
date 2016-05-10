using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Controllers
{
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
            if (@class == null)
            {
                return NotFound();
            }

            return Ok(@class);
        }

        // POST: api/Classes
        [ResponseType(typeof(Class))]
        [Route("api/Classes")]
        [ValidateModel]
        public IHttpActionResult PostClass(ClassEntryModel classModel)
        {
            var newClass = _classesServices.Map(classModel);
            _classesServices.Add(newClass);
            return Ok(newClass);
        }

        // DELETE: api/Classes/5
        [Route("api/Classes/{id}")]
        [ResponseType(typeof(Class))]
        public IHttpActionResult DeleteClass(long id)
        {
            var @class = _classesServices.Delete(id);
            if (@class == null)
            {
                return NotFound();
            }
            return Ok(@class);
        }
    }
}