using System;

namespace VinculacionBackend.Data.Exceptions
{
    public class InvalidSectionOrProjectException : Exception
    {
        public InvalidSectionOrProjectException(string message) : base(message)
        {
            
        }

    }
}
