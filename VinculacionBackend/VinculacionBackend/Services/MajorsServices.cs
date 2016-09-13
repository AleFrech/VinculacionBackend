using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Exceptions;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Services
{
    public class MajorsServices : IMajorsServices
    {
        private readonly IMajorRepository _majorRepository;

        public MajorsServices(IMajorRepository majorRepository)
        {
            _majorRepository = majorRepository;
        }

        public Major Find(string majorId)
        {
            var major = _majorRepository.GetMajorByMajorId(majorId);
            if(major==null)
                throw new NotFoundException("No se encontro la carrera");
            return major;
        }

        public IQueryable<Major> All()
        {
            return _majorRepository.GetAll();
        }

        public IQueryable<Major> GetByProject(long projectId)
        {
            return _majorRepository.GetMajorsByProject(projectId);
        }

        public Major FindByName(string name)
        {
            return _majorRepository.GetMajorByName(name);
        }
    }
}