using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Models
{
    public class HourEntryModel
    {
        public string NumberId { get; set; }

        public long SectionId { get; set; }

        public long ProyectId{ get; set; }
       
        public int Hour { get; set; }
    }
}