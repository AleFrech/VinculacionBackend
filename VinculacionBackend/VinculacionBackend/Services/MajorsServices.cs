using System.Linq;
using VinculacionBackend.Entities;
using VinculacionBackend.Repositories;

namespace VinculacionBackend.Services
{
    public class MajorsServices
    {
        readonly MajorRepository _majorRepository = new MajorRepository();
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