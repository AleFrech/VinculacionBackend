using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Entities
{
    public class SectionProyect
    {
        public long Id { get; set; }
        public Section Section { get; set; }
        public Proyect Proyect { get; set; }

    }
}
