using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProfessorsController : ApiController
    {

        private readonly IProfessorsServices _professorsServices;
        private readonly IEmail _email;
        private readonly IEncryption _encryption;

        public ProfessorsController(IProfessorsServices professorsServices, IEmail email, IEncryption encryption)
        {
            _professorsServices = professorsServices;
            _encryption = encryption;
            _email = email;
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


        //Put: api/Professors/Verified
        [ResponseType(typeof(User))]
        [Route("api/Professors/Verified")]
        [ValidateModel]
        public IHttpActionResult PostAcceptVerified(VerifiedProfessorModel model)
        {
            model.AccountId = HttpContext.Current.Server.UrlDecode(model.AccountId);
            _professorsServices.VerifyProfessor(model);
            return Ok();
        }


        // POST: api/Professors
        [ResponseType(typeof(User))]
        [Route("api/Professors")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostUser(ProfessorEntryModel professorModel)
        {
            var professor= new User();
            _professorsServices.Map(professor,professorModel);
            _professorsServices.AddProfessor(professor);
            var accountIdParameter = _encryption.Encrypt(professor.AccountId);
            _email.Send(professor.Email
            ,"Hacer click en el siguiente link para establecer su contraseña : http://fiasps.unitec.edu:8096/registro-maestro/" + HttpContext.Current.Server.UrlEncode(accountIdParameter)
            ,"Vinculacion");
            return Ok(professor);
        }

        [ResponseType(typeof(User))]
        [Route("api/Professors/{accountId}")]
        [ValidateModel]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutProfessor(string accountId, ProfessorUpdateModel model)
        {

            var professor = _professorsServices.UpdateProfessor(accountId, model);
            return Ok(professor);
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