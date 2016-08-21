using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface ISectionProjectServices
    {
        SectionProjectInfoModel GetInfo(long id);
        void Approve(long sectionProjectId);
    }
}
