using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;
using System.Web.Http.Cors;

namespace VinculacionBackend.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HoursController : ApiController
    {
        private VinculacionContext db = new VinculacionContext();

        // POST: api/Hours
        [ResponseType(typeof(Hour))]
        [Route("api/Hours")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult PostHour(HourEntryModel hourModel)
        {
            if (!ModelState.IsValid || hourModel== null)
            {
                return BadRequest(ModelState);
            }
            if (!CheckHourModel(hourModel))
            {
                return InternalServerError(new Exception("Uno o mas campos vacios"));
            }
            var sectionProjectRel = db.SectionProjectsRels.Include(x => x.Project).Include(y => y.Section).FirstOrDefault(z => z.Section.Id == hourModel.SectionId && z.Project.Id == hourModel.ProjectId);
            var user = db.Users.FirstOrDefault(x => x.AccountId == hourModel.AccountId);
            if (user != null && sectionProjectRel != null)
            {
                var hour = new Hour();
                hour.Amount = hourModel.Hour;
                hour.SectionProject = sectionProjectRel;
                hour.User = user;
                db.Hours.Add(hour);
                db.SaveChanges();
                return Ok(hour);
            }
            else
            {
                return NotFound();
            } 
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool HourExists(long id)
        {
            return db.Hours.Count(e => e.Id == id) > 0;
        }

        private bool CheckHourModel(HourEntryModel model)
        {
            bool isvalid = (model.AccountId != null) && (model.Hour >= 0) && (model.ProjectId >= 1) &&
                (model.SectionId >=1);
            return isvalid;
        }
    }
}