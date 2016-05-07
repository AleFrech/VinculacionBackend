using System.Collections;
using System.Collections.Generic;

namespace VinculacionBackend.Models
{
    public class HourReportModel
    {
        public int TotalHours;
        public IEnumerable<HourReportUnitModel> Projects;
    }
}