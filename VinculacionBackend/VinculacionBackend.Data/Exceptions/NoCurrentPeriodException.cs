using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinculacionBackend.Data.Exceptions
{
    public class NoCurrentPeriodException : Exception
    {
        public NoCurrentPeriodException() : base("No se puede ejecutar la acción debido a que no hay un periodo actual asignado")
        {

        }
    }
}
