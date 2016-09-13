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
        public HttpContext GenerateReport(DataTable[] dt, string workSheetName)
        {
            var excel = new XLWorkbook();
            var workSheet =  excel.Worksheets.Add(dt[0], workSheetName);
            for(int i = 1; i < dt.Count(); i++)
            {
                var name = "A" + (dt[i - 1].Rows.Count + 3);
                workSheet.Cell(name).InsertTable(dt[i]);
            }
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