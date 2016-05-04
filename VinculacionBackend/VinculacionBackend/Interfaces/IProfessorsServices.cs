using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IProfessorsServices
    {
        User Map(ProfessorEntryModel professorModel);
        void AddProfessor(User user);
        User Find(string accountId);
        User DeleteProfessor(string accountId);
        IQueryable<User> GetProfessors();
    }
}