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
        HourRepository _hourRepository = new HourRepository();
        public Hour Add(HourEntryModel model)
        {
            return _hourRepository.InsertHourFromModel(model);
        }
    }
}