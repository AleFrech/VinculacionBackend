using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]


    public class ReportsController : ApiController
    {

        protected DataTable BindDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserId", typeof(Int32));
            dt.Columns.Add("UserName", typeof(string));
            dt.Columns.Add("Education", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Rows.Add(1, "SureshDasari", "B.Tech", "Chennai");
            dt.Rows.Add(2, "MadhavSai", "MBA", "Nagpur");
            dt.Rows.Add(3, "MaheshDasari", "B.Tech", "Nuzividu");
            dt.Rows.Add(4, "Rohini", "MSC", "Chennai");
            dt.Rows.Add(5, "Mahendra", "CA", "Guntur");
            dt.Rows.Add(6, "Honey", "B.Tech", "Nagpur");
            return dt;
        }

        [Route("api/Reports/CostsReport")]
        public IHttpActionResult GetReport()
        {
            //var dt = BindDatatable();
            //var excel = new XLWorkbook();
            //excel.Worksheets.Add(dt,"F1");
            //var ms = new MemoryStream();
            //excel.SaveAs(ms);
            //HttpContext context = HttpContext.Current;
            //context.Response.Buffer = true;
            //context.Response.Charset = "";
            //context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //context.Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
            //ms.WriteTo(context.Response.OutputStream);
            //context.Response.Flush();
            //context.Response.End();
            return Ok();
        }
    
    }
}