using System.Linq;

namespace VinculacionBackend
{
    public static class EntityExistanceManager
    {
        public static bool EmailExists(string email)
        {
            var context = new Database.VinculacionContext();
            return context.Users.Any(x => x.Email == email);
        }
        
        public static bool AccountNumberExists(string accountNumber) 
        {
            var context = new Database.VinculacionContext();
            return context.Users.Any(x=>x.AccountId == accountNumber);
        }
    }
}