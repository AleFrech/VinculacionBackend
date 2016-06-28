using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;
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


    }
}