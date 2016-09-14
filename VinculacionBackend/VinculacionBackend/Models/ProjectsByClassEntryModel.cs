using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Models
{
    public class ProjectsByClassEntryModel
    {
        public string IdProyecto { get; set; }
        public string Nombre { get; set; }
        public string Beneficiario { get; set; }
        public string Maestro { get; set; }
        public int Periodo { get; set; }
        public int Anio { get; set; }

    }
}
