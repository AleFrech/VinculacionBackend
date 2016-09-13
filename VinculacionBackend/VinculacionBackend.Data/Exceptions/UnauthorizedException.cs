using System;

namespace VinculacionBackend.Data.Exceptions
{
    public class UnauthorizedException:Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
    }
}