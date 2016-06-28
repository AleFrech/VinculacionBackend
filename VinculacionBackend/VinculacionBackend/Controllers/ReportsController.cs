using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]


    public class ReportsController : ApiController
    {
        private readonly IFacultiesServices _facultiesServices;
        private readonly ISheetsReportsServices _reportsServices;
        private readonly IStudentsServices _studentServices;

        private readonly IProjectServices _projectServices;


        public ReportsController(IFacultiesServices facultiesServices, ISheetsReportsServices reportsServices, IStudentsServices studentsServices, IProjectServices projectServices)
        {
            _facultiesServices = facultiesServices;
            _reportsServices = reportsServices;
            _projectServices = projectServices;
            _studentServices = studentsServices;
        }


        [Route("api/Reports/CostsReport/{year}")]
        public IHttpActionResult GetCostsReport(int year)
        {
            var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesCostReport(year),
                "Reporte de Costos por Facultad");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/ProjectsReport")]
        public IHttpActionResult GetProjectCountByMajorReport()
        {
            var context = _reportsServices.GenerateReport(_projectServices.CreateProjectsByMajor(2013),
                "Reporte de Proyectos por Carrera");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/HoursReport")]
        public IHttpActionResult GetHoursReport()
        {
            var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesHourReport(),
                "Reporte de Horas por Facultad");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/StudentsReport/{year}")]
        public IHttpActionResult GetStudentsReport(int year)
        {
            var context = _reportsServices.GenerateReport(_studentServices.CreateStudentReport(year),
                "Reporte de Alumnos");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

    }
}