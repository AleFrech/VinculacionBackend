using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Security;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HoursController : ApiController
    {
        private readonly HoursServices _hoursServices = new HoursServices(new HourRepository());

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostHour(HourEntryModel hourModel)
        {
            var hour = _hoursServices.Add(hourModel);
            if (hour!=null)
            {
                return Ok(hour);
            }
            return NotFound();
        }
    }
}