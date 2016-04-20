using System.Linq;
using VinculacionBackend.Entities;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Services
{
    public class MajorsServices
    {
        private readonly IMajorRepository _majorRepository;

        public MajorsServices(IMajorRepository majorRepository)
        {
            _majorRepository = majorRepository;
        }

        public Major Find(string majorId)
        {
            return _majorRepository.GetMajorByMajorId(majorId);
        }

        public IQueryable<Major> All()
        {
            return _majorRepository.GetAll();
        }
    }
}