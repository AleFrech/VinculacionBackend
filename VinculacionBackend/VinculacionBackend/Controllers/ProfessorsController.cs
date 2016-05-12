using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    public class ProfessorsController : ApiController
    {

        private readonly IProfessorsServices _professorsServices;

        public ProfessorsController(IProfessorsServices professorsServices)
        {
            _professorsServices = professorsServices;
        }

        [Route("api/Professors")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [EnableQuery]
        // GET: api/Professors
        public IQueryable<User> GetUsers()
        {
            return _professorsServices.GetProfessors();
        }

        // GET: api/Professors/5
        [Route("api/Professors/{accountId}")]
        [CustomAuthorize(Roles = "Admin,Professor,Student")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string accountId)
        {
            var professor = _professorsServices.Find(accountId);
            return Ok(professor);
        }


        // POST: api/Professors
        [ResponseType(typeof(User))]
        [Route("api/Professors")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostUser(ProfessorEntryModel professorModel)
        {
            var newProfessor = _professorsServices.Map(professorModel);
            _professorsServices.AddProfessor(newProfessor);
            return Ok(newProfessor);
        }

        // DELETE: api/Professors/5
        [ResponseType(typeof(User))]
        [Route("api/Professors/{accountId}")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult DeleteUser(string accountId)
        {

            var professor = _professorsServices.DeleteProfessor(accountId);
            return Ok(professor);
        }


    }
}