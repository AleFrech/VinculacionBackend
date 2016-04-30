using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinculacionBackend.Data.Entities
{
    public class ProjectMajor
    {
        public long Id { get; set; }
        public Project Project { get; set; }
        public Major Major { get; set; }
    }
}
