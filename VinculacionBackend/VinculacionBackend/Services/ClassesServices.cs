using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Exceptions;
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
            if (@class == null)
                throw new NotFoundException("No se encontro la clase");
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
            var @class = _classesRepository.Get(id);
            if (@class !=null)
            return @class;
            throw new NotFoundException("No se encontro la clase");
        }

        public void Map(Class @class,ClassEntryModel classModel)
        {
            @class.Name = classModel.Name;
            @class.Code = classModel.Code;
        }

        public Class UpdateClass(long classId, ClassEntryModel classModel)
        {
            var @class = _classesRepository.Get(classId);
            if (@class == null)
                throw new NotFoundException("No se encontro la clase");
             Map(@class,classModel);
            _classesRepository.Update(@class);
            _classesRepository.Save();
            return @class;
        }
    }
}