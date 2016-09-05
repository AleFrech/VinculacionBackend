using System;

namespace VinculacionBackend.Data.Exceptions
{
    public class StudentAlreadyRegisteredInClassException : Exception
    {
        public StudentAlreadyRegisteredInClassException(string message) :base(message)
        {

        }
    }
}
