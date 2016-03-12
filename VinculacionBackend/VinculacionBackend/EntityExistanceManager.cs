using System.Linq;

namespace VinculacionBackend
{
    public static class EntityExistanceManager
    {
        public static bool EmailExists(string email)
        {
            var context = new Database.VinculacionContext();
            var found = context.Users.FirstOrDefault(x => x.Email == email);
            return found != null;
        }
        
        public static bool AccountNumberExists(string accountNumber) 
        {
            var context = new Database.VinculacionContext();
            var found = context.Users.FirstOrDefault(x => x.AccountId == accountNumber);
            return found != null;
        }
    }
}