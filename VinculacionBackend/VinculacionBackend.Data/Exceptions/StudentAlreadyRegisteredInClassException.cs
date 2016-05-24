using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinculacionBackend.Data.Exceptions
{
    public class StudentAlreadyRegisteredInClassException : Exception
    {
        public StudentAlreadyRegisteredInClassException(string message) : base(message)
        {

        }
    }
}
