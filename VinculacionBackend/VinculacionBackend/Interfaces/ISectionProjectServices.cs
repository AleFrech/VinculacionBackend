using System.Linq;
﻿using VinculacionBackend.Data.Entities;
using VinculacionBackend.Models;

namespace VinculacionBackend.Interfaces
{
    public interface ISectionProjectServices
    {
        IQueryable<SectionProject> GetUnapproved();
        SectionProject GetInfo(long sectionprojectId);
        void Approve(long sectionprojectId);
        SectionProject AddOrUpdate(SectionProjectEntryModel sectionProjectEntryModel);
    }
}
