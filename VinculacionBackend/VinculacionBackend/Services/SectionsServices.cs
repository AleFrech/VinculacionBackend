using System;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Exceptions;
using VinculacionBackend.Data.Interfaces;
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
        private readonly IStudentsServices _studentServices;

        public SectionsServices(ISectionRepository sectionsRepository, IProfessorsServices professorsServices, IClassesServices classServices, IPeriodsServices periodsServices)
        {
            _sectionsRepository = sectionsRepository;
            _professorsServices = professorsServices;
            _classServices = classServices;
            _periodsServices = periodsServices;
            
        }

        public IQueryable<Section> All()
        {
           return _sectionsRepository.GetAll();
        }

        public IQueryable<Section> AllByUser(long userId, string[] roles)
        {
            if (roles.Contains("Admin"))
            {
                return _sectionsRepository.GetAll();
            }
            else if (roles.Contains("Professor"))
            {
                return _sectionsRepository.GetAllByProfessor(userId);
            }
            else if (roles.Contains("Student"))
            {
                return _sectionsRepository.GetAllByStudent(userId);
            }
            throw new UnauthorizedException("No tiene permiso");
        }

        public IQueryable<Section> GetCurrentPeriodSectionsByUser(long userId, string role)
        {
            var currentPeriod = _periodsServices.GetCurrentPeriod();
            if (currentPeriod == null)
                throw new NoCurrentPeriodException();
            if (role.Equals("Admin"))
            {
                return _sectionsRepository.GetAll().Where(x => x.Period.Id == currentPeriod.Id).Distinct();
            }
            else if (role.Equals("Professor"))
            {
                return _sectionsRepository.GetAllByProfessor(userId).Where(a=>a.Period.Id == currentPeriod.Id).Distinct();
            }
            else if (role.Equals("Student"))
            {
                return _sectionsRepository.GetAllByStudent(userId).Where(x => x.Period.Id == currentPeriod.Id).Distinct();
            }
            throw new UnauthorizedException("No tiene permiso");
        }

        public IQueryable<Section> GetSectionsByProject(long projectId, string role, long userId)
        {
            if (role.Equals("Admin"))
            {
                return _sectionsRepository.GetSectionsByProject(projectId);
            }
            else if (role.Equals("Professor"))
            {
                return _sectionsRepository.GetSectionsByProject(projectId).Where(x=>x.User.Id==userId);
            }
            else if (role.Equals("Student"))
            {
                return _sectionsRepository.GetSectionsByProject(projectId).Join(_sectionsRepository.GetSectionsUsersRels(), s => s.Id, su => su.Section.Id, (s,su)=> su).Where(u => u.User.Id == userId).Select(x => x.Section).Include(p => p.Period).Include( p=> p.User).Include(p => p.Class);

            }
            throw new UnauthorizedException("No tiene permiso");
        }

        public IQueryable<Section> GetCurrentPeriodSections()
        {
            var currentPeriod = _periodsServices.GetCurrentPeriod();
            var sections= _sectionsRepository.GetAll().Where(x => x.Period.Id ==currentPeriod.Id);

            return sections;
        }

        public Section Delete(long sectionId)
        {
            var section = _sectionsRepository.Delete(sectionId);
            if(section ==null)
                throw new NotFoundException("No se encontro la seccion");
            _sectionsRepository.Save();
            return section;
            
        }

        public void Map(Section section,SectionEntryModel sectionModel)
        {
            section.Code = sectionModel.Code;
            if (section.Class==null ||section.Class.Id!=sectionModel.ClassId)
                section.Class = _classServices.Find(sectionModel.ClassId);
            if (section.User==null || section.User.AccountId != sectionModel.ProffesorAccountId)
                section.User =_professorsServices.Find(sectionModel.ProffesorAccountId);
            section.Period = _periodsServices.GetCurrentPeriod();
            if (section.Period == null)
                throw new NoCurrentPeriodException();
        }

        public void PutMap(Section section, SectionEntryModel sectionModel)
        {
            section.Code = sectionModel.Code;
            if (section.Class == null || section.Class.Id != sectionModel.ClassId)
                section.Class = _classServices.Find(sectionModel.ClassId);
            if (section.User == null || section.User.AccountId != sectionModel.ProffesorAccountId)
                section.User = _professorsServices.Find(sectionModel.ProffesorAccountId);
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
            PutMap(tmpSection,model);
            tmpSection.Id = sectionId;
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

        public object GetSectionStudentsHour(long sectionId,long projectId )
        {
            return _sectionsRepository.GetSectionStudentsHours(sectionId,projectId);
        }

        public void RebuildSectionStudentRelationships(SectionStudentModel model)
        {
            _sectionsRepository.ClearSectionStudents(model.SectionId);
            _sectionsRepository.Save();
            _sectionsRepository.AssignStudents(model.SectionId, model.StudenstIds);    
            _sectionsRepository.Save();
        }
    }
}
