using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class ClassesServices:IClassesServices

    {
        private readonly IClassRepository _classesRepository;
        private readonly IMajorRepository _majorRepository;

        public ClassesServices(IClassRepository classesRepository, IMajorRepository majorRepository)
        {
            _classesRepository = classesRepository;
            _majorRepository = majorRepository;
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

        public void Add(Class @class, List<string> majorIds)
        {
            _classesRepository.InsertClass(@class, majorIds);
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