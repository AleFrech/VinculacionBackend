using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class HoursServices
    {
        public Hour Map(Hour hour, HourEntryModel hourModel)
        {
            Hour newHour = new Hour();
            newHour.Amount = hourModel.Hour;
            newHour.SectionProject = sectionProjectRel;
            newHour.User = user;

            return hour;
        }
    }
}