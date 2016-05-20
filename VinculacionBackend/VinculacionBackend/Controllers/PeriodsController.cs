using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using System.Web.OData;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

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

        // POST: api/Periods
        [ResponseType(typeof(Period))]
        [Route("api/Periods")]
        [ValidateModel]
        public IHttpActionResult PostPeriod(PeriodEntryModel periodModel)
        {
            var newPeriod = _periodsServices.Map(periodModel);
            _periodsServices.Add(newPeriod);
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