using System;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

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
            var professor = _professorRepository.GetByAccountId(accountId);
            if(professor==null)
                throw new NotFoundException("No se encontro el profesor");
            return professor;
        }

        public User DeleteProfessor(string accountId)
        {
            var professor = _professorRepository.DeleteByAccountNumber(accountId);
            if(professor==null)
                throw new NotFoundException("No se encontro el profesor");
            _professorRepository.Save();
            return professor;
        }

        public IQueryable<User> GetProfessors()
        {
            return _professorRepository.GetAll();
        }
    }
}