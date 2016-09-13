using System;

namespace VinculacionBackend.Data.Exceptions
{
    public class HoursAlreadyApprovedException : Exception
{
        public HoursAlreadyApprovedException(string message) : base(message)
    {

        }
    }
}