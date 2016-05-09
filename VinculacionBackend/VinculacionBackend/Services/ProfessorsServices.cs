using System;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using IEncryption = VinculacionBackend.Data.Interfaces.IEncryption;

namespace VinculacionBackend.Services
{
    public class ProfessorsServices:IProfessorsServices
    {
        private readonly IProfessorRepository _professorRepository;
        private readonly IEncryption _encryption;

        public ProfessorsServices(IProfessorRepository professorRepository, IEncryption encryption)
        {
            _professorRepository = professorRepository;
            _encryption = encryption;
        }

        public User Map(ProfessorEntryModel professorModel)
        {
            var newProfessor = new User();
            newProfessor.AccountId = professorModel.AccountId;
            newProfessor.Name = professorModel.Name;
            newProfessor.Password = _encryption.Encrypt(professorModel.Password);
            newProfessor.Major = null;
            newProfessor.Campus = professorModel.Campus;
            newProfessor.Email = professorModel.Email;
            newProfessor.Status = Status.Verified;
            newProfessor.CreationDate = DateTime.Now;
            newProfessor.ModificationDate = DateTime.Now;
            return newProfessor;
        }

        public void AddProfessor(User professor)
        {
            _professorRepository.Insert(professor);
            _professorRepository.Save();
        }

        public User Find(string accountId)
        {
            return _professorRepository.GetByAccountId(accountId);
        }

        public User DeleteProfessor(string accountId)
        {
            var professor = _professorRepository.DeleteByAccountNumber(accountId);
            _professorRepository.Save();
            return professor;
        }

        public IQueryable<User> GetProfessors()
        {
            return _professorRepository.GetAll();
        }
    }
}