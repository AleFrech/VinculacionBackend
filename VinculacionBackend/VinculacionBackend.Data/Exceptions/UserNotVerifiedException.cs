using System;

namespace VinculacionBackend.Data.Exceptions
{
    public class UserNotVerifiedException: Exception
    {
        public UserNotVerifiedException(string message) : base(message)
        {

        }
    }
}
