using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Entities
{
    public class SectionProject
    {
        public long Id { get; set; }
        public Section Section { get; set; }
        public Project Project { get; set; }

    }
}
