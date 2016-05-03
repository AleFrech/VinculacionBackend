using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Services
{
    public class SectionsServices : ISectionsServices
    {
        private readonly ISectionRepository _sectionsRepository;

        public SectionsServices(ISectionRepository sectionsRepository)
        {
            _sectionsRepository = sectionsRepository;
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