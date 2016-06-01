using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinculacionBackend.Data.Entities
{
    public class MajorClass
    {
        public long Id { get; set; }
        public Major Major { get; set; }
        public Class Class { get; set; }
    }
}
