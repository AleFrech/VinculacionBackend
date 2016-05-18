using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IStudentsServices
    {
        User Map(UserEntryModel userModel);
        void Add(User user);
        User Find(string accountId);
        IQueryable<User> ListbyStatus(string status);
        User RejectUser(string accountId);
        User ActivateUser(string accountId);
        User VerifyUser(string accountId);
        User DeleteUser(string accountId);
        IQueryable<User> AllUsers();
        int GetStudentHours(string accountId);
        User FindByEmail(string email);
    }
}
