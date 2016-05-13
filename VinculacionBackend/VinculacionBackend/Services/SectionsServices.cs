using System.CodeDom;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class SectionsServices : ISectionsServices
    {
        private readonly ISectionRepository _sectionsRepository;
        private readonly IStudentsServices _studentServices;
        private readonly IProfessorsServices _professorsServices;
        private readonly IClassesServices _classServices;
        private readonly IPeriodsServices _periodsServices;

        public SectionsServices(ISectionRepository sectionsRepository, IStudentsServices studentServices, IProfessorsServices professorsServices, IClassesServices classServices, IPeriodsServices periodsServices)
        {
            _sectionsRepository = sectionsRepository;
            _studentServices = studentServices;
            _professorsServices = professorsServices;
            _classServices = classServices;
            _periodsServices = periodsServices;
            ;
        }

        public IQueryable<Section> All()
        {
           return _sectionsRepository.GetAll();
        }
        
        public Section Delete(long sectionId)
        {
            var section = _sectionsRepository.Delete(sectionId);
            if(section ==null)
                throw new NotFoundException("No se encontro la seccion");
            _sectionsRepository.Save();
            return section;
            
        }

        public Section Map(SectionEntryModel sectionModel)
        {
            var newSection=new Section();
            newSection.Code = sectionModel.Code;
            newSection.User = _professorsServices.Find(sectionModel.ProffesorAccountId);
            newSection.Class = _classServices.Find(sectionModel.ClassId);
            newSection.Period = _periodsServices.Find(sectionModel.PeriodId);
            return newSection;

        }

        public bool RemoveStudent(SectionStudentModel model)
        {
            var section = Find(model.SectionId);
            var student = _studentServices.Find(model.StudentId);
            _sectionsRepository.RemoveStudent(section.Id, student.Id);
            _sectionsRepository.Save();
            return true;
        }

        public void Add(Section section)
        {
            _sectionsRepository.Insert(section);
            _sectionsRepository.Save();
        }

        public Section Find(long id)
        {
            var section = _sectionsRepository.Get(id);
            if (section == null)
                throw new NotFoundException("No se encontro la seccion");

            return section;

        }

        public bool AssignStudent(SectionStudentModel model)
        {
            var section = Find(model.SectionId);
            var student = _studentServices.Find(model.StudentId);
            _sectionsRepository.AssignStudent(section.Id, student.Id);
            _sectionsRepository.Save();
            return true;
        }
        
       
    }
}