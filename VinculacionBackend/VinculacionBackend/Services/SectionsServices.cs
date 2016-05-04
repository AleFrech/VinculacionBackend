using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class SectionsServices : ISectionsServices
    {
        private readonly ISectionRepository _sectionsRepository;
        private readonly IProfessorRepository _professorRepository;
        private  VinculacionContext _db = new VinculacionContext();

        public SectionsServices(ISectionRepository sectionsRepository, IProfessorRepository professorRepository)
        {
            _sectionsRepository = sectionsRepository;
            _professorRepository = professorRepository;
        }

        public IQueryable<Section> All()
        {
           return _sectionsRepository.GetAll();
        }
        

        public Section Delete(long sectionId)
        {
            var section = _sectionsRepository.Delete(sectionId);
            _sectionsRepository.Save();
            return section;
            
        }

        public Section Map(SectionEntryModel sectionModel)
        {
            var newSection=new Section();
            newSection.Code = sectionModel.Code;
            newSection.User = _professorRepository.GetByAccountId(sectionModel.ProffesorAccountId);
            newSection.Class = _db.Classes.FirstOrDefault(x => x.Id == sectionModel.ClassId);
            newSection.Period = _db.Periods.FirstOrDefault(x => x.Id == sectionModel.PeriodId);
            return newSection;

        }

        public void Add(Section section)
        {
            _sectionsRepository.Insert(section);
            _sectionsRepository.Save();
        }

        public Section Find(long id)
        {
           return  _sectionsRepository.Get(id);
                 
        }
    }
}