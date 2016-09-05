using System;

namespace VinculacionBackend.Data.Exceptions
{
    public class InvalidUsernameOrPasswordException : Exception
    {
        public InvalidUsernameOrPasswordException(string message) : base(message)
        {
            
        }
    }
}
