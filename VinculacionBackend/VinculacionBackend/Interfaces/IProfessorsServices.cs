using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface IProfessorsServices
    {
        void Map(User professor,ProfessorEntryModel professorModel);
        void AddProfessor(User user);
        User Find(string accountId);
        User DeleteProfessor(string accountId);
        IQueryable<User> GetProfessors();
        User UpdateProfessor(string accountId, ProfessorUpdateModel model);
        void VerifyProfessor(VerifiedProfessorModel model);
    }
}