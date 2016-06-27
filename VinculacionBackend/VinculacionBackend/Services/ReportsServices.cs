using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using ClosedXML.Excel;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Services
{
    public class ReportsServices  :ISheetsReportsServices
    {

        public HttpContext GenerateReport(DataTable dt,string workSheet)
        {
            var excel = new XLWorkbook();
            excel.Worksheets.Add(dt, workSheet);
            var ms = new MemoryStream();
            excel.SaveAs(ms);
            HttpContext context = HttpContext.Current;
            context.Response.Buffer = true;
            context.Response.Charset = "";
            context.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            context.Response.AddHeader("content-disposition", "attachment;filename=Reportes.xlsx");
            ms.WriteTo(context.Response.OutputStream);
            return context;
        }

    }
}