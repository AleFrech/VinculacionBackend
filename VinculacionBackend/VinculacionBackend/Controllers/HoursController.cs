using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HoursController : ApiController
    {
        private readonly IHoursServices _hoursServices;

        public HoursController(IHoursServices hoursServices)
        {
            _hoursServices = hoursServices;
        }

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostHour(HourEntryModel hourModel)
        {
            var hour = _hoursServices.Add(hourModel, HttpContext.Current.User.Identity.Name);
            return Ok(hour); 
        }


        [ResponseType(typeof(Hour))]
        [Route("api/Hours/AddMany")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostManyHours(HourCollectionEntryModel hourModel)
        {
            _hoursServices.AddMany(hourModel, HttpContext.Current.User.Identity.Name);
            return Ok();
        }

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours/{HourId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PutHour(long hourId,HourEntryModel hourModel)
        {
            var hour = _hoursServices.Update(hourId,hourModel);
            return Ok(hour);
        }
    }
}