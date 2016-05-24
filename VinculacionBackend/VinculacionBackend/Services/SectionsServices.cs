using System.Linq;
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
        private readonly IProfessorsServices _professorsServices;
        private readonly IClassesServices _classServices;
        private readonly IPeriodsServices _periodsServices;

        public SectionsServices(ISectionRepository sectionsRepository, IProfessorsServices professorsServices, IClassesServices classServices, IPeriodsServices periodsServices)
        {
            _sectionsRepository = sectionsRepository;
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

        public void Add(Section section)
        {
            _sectionsRepository.Insert(section);
            _sectionsRepository.Save();
        }


        public Section UpdateSection(long sectionId,SectionEntryModel model)
        {
            var tmpSection = _sectionsRepository.Get(sectionId);
            if (tmpSection == null)
                throw new NotFoundException("No se encontro la seccion");
            tmpSection = Map(model);
            _sectionsRepository.Update(tmpSection);
            _sectionsRepository.Save();
            return tmpSection;
        }


        public bool AssignStudents(SectionStudentModel model)
        {
            _sectionsRepository.AssignStudents(model.SectionId,model.StudenstIds);
            _sectionsRepository.Save();
            return true;
        }


        public bool RemoveStudents(SectionStudentModel model)
        {
            _sectionsRepository.RemoveStudents(model.SectionId,model.StudenstIds);
            _sectionsRepository.Save();
            return true;
        }

        public IQueryable<User> GetSectionStudents(long sectionId)
        {
            return _sectionsRepository.GetSectionStudents(sectionId);
        }

        public IQueryable<Project> GetSectionsProjects(long sectionId)
        {
            return _sectionsRepository.GetSectionProjects(sectionId);
        }

        public Section Find(long id)
        {
            var section = _sectionsRepository.Get(id);
            if (section == null)
                throw new NotFoundException("No se encontro la seccion");

            return section;

        }       
    }
}