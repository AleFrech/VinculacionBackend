using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class ClassesServices:IClassesServices

    {
        private readonly IClassRepository _classesRepository;

        public ClassesServices(IClassRepository classesRepository)
        {
            _classesRepository = classesRepository;
        }
        public IQueryable<Class> All()
        {
            return _classesRepository.GetAll();
        }

        public Class Delete(long id)
        {
            var @class = _classesRepository.Delete(id);
            _classesRepository.Save();
            return @class;
        }

        public void Add(Class @class)
        {
           _classesRepository.Insert(@class);
            _classesRepository.Save();
        }

        public Class Find(long id)
        {
            return _classesRepository.Get(id);
        }

        public Class Map(ClassEntryModel classModel)
        {
            var newClass =  new Class();
            newClass.Name = classModel.Name;
            return newClass;
        }
    }
}