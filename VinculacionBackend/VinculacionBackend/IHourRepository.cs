using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend
{
    interface IHourRepository : IRepository<Hour> 
    {
        Hour InsertHourFromModel(HourEntryModel model);
    }
}
