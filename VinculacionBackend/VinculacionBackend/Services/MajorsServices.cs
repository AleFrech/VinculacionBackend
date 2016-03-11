using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Entities;

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
            return _majorRepository.GetAll() as IQueryable<Major>;
        }
    }
}