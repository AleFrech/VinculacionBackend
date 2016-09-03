using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinculacionBackend.Data.Exceptions
{
    public class InvalidPerioNumberException : Exception
    {
        public InvalidPerioNumberException(string message) : base(message)
        {
            
        }
    }
}
