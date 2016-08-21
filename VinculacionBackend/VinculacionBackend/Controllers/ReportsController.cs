using System.IO;
using System.Web.Http;
using System.Web.Http.Cors;
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
            var context = _reportsServices.GenerateReport(_projectServices.CreateProjectsByMajor(),
                "Proyectos por Carrera");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/HoursReport/{year}")]
        public IHttpActionResult GetHoursReport(int year)
        {
            var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesHourReport(year),
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

        [Route("api/Reports/ProjectsByClassReport/{classId}")]
        public IHttpActionResult GetProjectsByClassReport(long classId)
        {
            var context = _reportsServices.GenerateReport(_projectServices.ProjectsByClass(classId), "Projectos Por "+_classesServices.Find(classId).Name);
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [Route("api/Reports/PeriodReport/{year}")]
        public IHttpActionResult GetPeriodReport(int year)
        {
            var context = _reportsServices.GenerateReport(_projectServices.CreatePeriodReport(year, 1),
                1 + " " + year);
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }

        [HttpPost()]
        public string UploadFiles()
        {
            int iUploadedCnt = 0;

            // DEFINE THE PATH WHERE WE WANT TO SAVE THE FILES.
            string sPath = "";
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Files/");

            System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;

            // CHECK THE FILE COUNT.
            for (int iCnt = 0; iCnt <= hfc.Count - 1; iCnt++)
            {
                System.Web.HttpPostedFile hpf = hfc[iCnt];

                if (hpf.ContentLength > 0)
                {
                    // CHECK IF THE SELECTED FILE(S) ALREADY EXISTS IN FOLDER. (AVOID DUPLICATE)
                    if (!File.Exists(sPath + Path.GetFileName(hpf.FileName)))
                    {
                        // SAVE THE FILES IN THE FOLDER.
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));
                        iUploadedCnt = iUploadedCnt + 1;
                    }
                }
            }

            // RETURN A MESSAGE (OPTIONAL).
            if (iUploadedCnt > 0)
            {
                return iUploadedCnt + " Files Uploaded Successfully";
            }
            else {
                return "Upload Failed";
            }
        }
    }
}