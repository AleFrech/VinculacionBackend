using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend
{
    public class EntityExistanceManager
    {
        public static bool EmailExists(string email)
        {
            var context = new VinculacionBackend.Database.VinculacionContext();
            return context.Students.Any(x => x.Email == email);
        }
    }
}