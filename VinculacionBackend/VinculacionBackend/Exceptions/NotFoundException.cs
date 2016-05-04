using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend
{
    public class NotFoundException:Exception
    {
        public NotFoundException(string message):base(message)
        {
            
        }
    }
}