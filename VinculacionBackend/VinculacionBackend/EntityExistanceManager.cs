using System.Linq;

namespace VinculacionBackend
{
    public static class EntityExistanceManager
    {
        public static bool EmailExists(string email)
        {
            var context = new Database.VinculacionContext();
            return context.Students.Any(x => x.Email == email);
        }
    }
}