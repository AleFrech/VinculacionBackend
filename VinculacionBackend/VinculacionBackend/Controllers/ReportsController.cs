using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Data;
using System.Collections.Generic;
using OfficeOpenXml;
using VinculacionBackend.Extensions;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ReportsController : ApiController
    {
        private readonly IFacultiesServices _facultiesServices;
        private readonly ISheetsReportsServices _reportsServices;
        private readonly IStudentsServices _studentServices;
        private readonly IClassesServices _classesServices;
        private readonly IProjectServices _projectServices;


        public ReportsController(IFacultiesServices facultiesServices, ISheetsReportsServices reportsServices, IStudentsServices studentsServices, IProjectServices projectServices, IClassesServices classesServices)
        {
            _facultiesServices = facultiesServices;
            _reportsServices = reportsServices;
            _projectServices = projectServices;
            _classesServices = classesServices;
            _studentServices = studentsServices;
        }


        [Route("api/Reports/CostsReport/{year}")]
        public IHttpActionResult GetCostsReport(int year)
        {
            var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesCostReport(year).ToDataTable(),
                "Reporte de Costos por Facultad");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/ProjectsByMajorReport")]
        public IHttpActionResult GetProjectCountByMajorReport()
        {
            var context = _reportsServices.GenerateReport(_projectServices.CreateProjectsByMajor().ToDataTable(),
                "Proyectos por Carrera");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/HoursReport/{year}")]
        public IHttpActionResult GetHoursReport(int year)
        {
            var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesHourReport(year).ToDataTable(),
                "Reporte de Horas por Facultad");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/StudentsReport/{year}")]
        public IHttpActionResult GetStudentsReport(int year)
        {
            var datatables = new DataTable[2];
            datatables[0] = _studentServices.CreateStudentReport(year).ToDataTable();
            datatables[1] = _studentServices.CreateHourNumberReport(year).ToDataTable();
            var context = _reportsServices.GenerateReport(datatables,
                "Reporte de Alumnos");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/ProjectsByClassReport/{classId}")]
        public IHttpActionResult GetProjectsByClassReport(long classId)
        {
            var context = _reportsServices.GenerateReport(_projectServices.ProjectsByClass(classId).ToDataTable(), "Projectos Por " + _classesServices.Find(classId).Name);
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/PeriodReport/{year}")]
        public IHttpActionResult GetPeriodReport(int year)
        {
            var context = _reportsServices.GenerateReport(_projectServices.CreatePeriodReport(year, 1).ToDataTable(),
                1 + " " + year);
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

     
    }
}