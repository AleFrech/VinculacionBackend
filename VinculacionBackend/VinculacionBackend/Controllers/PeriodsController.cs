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
    public class PeriodsController : ApiController
    {
        private readonly IPeriodsServices _periodsServices;

        public PeriodsController(IPeriodsServices periodsServices)
        {
            _periodsServices = periodsServices;
        }

        // GET: api/Periods
        [Route("api/Periods")]
        [EnableQuery]
        public IQueryable<Period> GetPeriods()
        {
            return _periodsServices.All();
        }

        // GET: api/Periods/5
        [ResponseType(typeof(Period))]
        [Route("api/Periods/{id}")]

        public IHttpActionResult GetPeriod(long id)
        {
            Period period = _periodsServices.Find(id);
            return Ok(period);
        }

        // GET: api/Periods/5
        [ResponseType(typeof(Period))]
        [Route("api/Periods/GetCurrentPeriod")]

        public IHttpActionResult GetCurrentPeriod()
        {
            return Ok (_periodsServices.GetCurrentPeriod());
        }

        // PUT: api/Periods/SetCurrentPeriod/5
        [ResponseType(typeof(Period))]
        [Route("api/Periods/SetCurrentPeriod/{periodId}")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutSetCurrentPeriod(long periodId)
        {
            var period=_periodsServices.SetCurrentPeriod(periodId);
            return Ok(period);
        }

        // POST: api/Periods
        [ResponseType(typeof(Period))]
        [Route("api/Periods")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostPeriod(PeriodEntryModel periodModel)
        {
            var newPeriod =new Period();
            _periodsServices.Map(newPeriod,periodModel);
            _periodsServices.Add(newPeriod);
            return Ok(newPeriod);
        }

        [ResponseType(typeof(Period))]
        [Route("api/Periods/{periodId}")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PustPeriod(long periodId,PeriodEntryModel periodModel)
        {

            var newPeriod = _periodsServices.UpdatePeriod(periodId,periodModel);
            return Ok(newPeriod);
        }

        // DELETE: api/Periods/5
        [Route("api/Periods/{id}")]
        [ResponseType(typeof(Period))]
        public IHttpActionResult DeletePeriod(long id)
        {
            var period = _periodsServices.Delete(id);
            return Ok(period);
        }
    }
}