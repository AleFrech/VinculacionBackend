using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinculacionBackend.Exceptions
{
    public class HoursAlreadyApprovedException : Exception
    {
        public HoursAlreadyApprovedException(string message) : base(message)
        {

        }
    }
}
