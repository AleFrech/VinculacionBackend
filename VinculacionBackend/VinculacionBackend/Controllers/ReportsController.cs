using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using VinculacionBackend.Models;
using System.Web.Http.Cors;
using System.Web.OData;
using ClosedXML.Excel;


namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]

    public class ReportsController : ApiController
    {
        protected DataTable BindDatatable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("UserId", typeof (Int32));
            dt.Columns.Add("UserName", typeof (string));
            dt.Columns.Add("Education", typeof (string));
            dt.Columns.Add("Location", typeof (string));
            dt.Rows.Add(1, "SureshDasari", "B.Tech", "Chennai");
            dt.Rows.Add(2, "MadhavSai", "MBA", "Nagpur");
            dt.Rows.Add(3, "MaheshDasari", "B.Tech", "Nuzividu");
            dt.Rows.Add(4, "Rohini", "MSC", "Chennai");
            dt.Rows.Add(5, "Mahendra", "CA", "Guntur");
            dt.Rows.Add(6, "Honey", "B.Tech", "Nagpur");
            return dt;
        }

        [Route("api/Reports/CostsReport")]
        public HttpResponseMessage GetReport()
        {
            var dt = BindDatatable();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dt, "Customers");
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    response.Content = new StreamContent(MyMemoryStream);

                }
            }
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "prueba"
            };
            return response;
        }
    }
}