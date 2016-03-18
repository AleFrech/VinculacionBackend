using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HoursController : ApiController
    {
        HoursServices _hoursServices = new HoursServices();
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