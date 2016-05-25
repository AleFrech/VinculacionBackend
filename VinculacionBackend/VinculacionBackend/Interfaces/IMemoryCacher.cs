using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Interfaces
{
    public interface IMemoryCacher
    {
        IEnumerable<Major> GetMajors(IMajorsServices majorsServices);
    }

}
