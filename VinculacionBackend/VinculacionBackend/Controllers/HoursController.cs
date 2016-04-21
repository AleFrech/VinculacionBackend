using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.Repositories;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HoursController : ApiController
    {
        public readonly IHoursServices HoursServices;

        public HoursController(IHoursServices hoursServices)
        {
            HoursServices = hoursServices;
        }

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostHour(HourEntryModel hourModel)
        {
            var hour = HoursServices.Add(hourModel);
            if (hour!=null)
            {
                return Ok(hour);
            }
            return NotFound();
        }
    }
}