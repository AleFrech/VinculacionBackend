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
        readonly HourRepository _hourRepository = new HourRepository();
        public Hour Add(HourEntryModel hourModel)
        {
            var hour = _hourRepository.InsertHourFromModel(hourModel);
            _hourRepository.Save();
            return hour;
        }
    }
}