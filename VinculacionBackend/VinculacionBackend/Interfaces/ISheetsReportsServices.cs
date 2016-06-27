using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace VinculacionBackend.Interfaces
{
     public interface ISheetsReportsServices
     {
         HttpContext GenerateReport(DataTable dt, string workSheet);
         HttpContext GenerateReport(DataTable[] dt, string workSheet);
    }
}
