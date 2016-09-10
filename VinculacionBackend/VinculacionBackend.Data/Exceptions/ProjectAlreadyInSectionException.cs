using System;

namespace VinculacionBackend.Data.Exceptions
{
    public class ProjectAlreadyInSectionException :Exception
    {
        public ProjectAlreadyInSectionException(string message) : base(message)
        {
            
        }
    }
}