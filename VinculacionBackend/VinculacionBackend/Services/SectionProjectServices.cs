using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Exceptions;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Services
{
    public class SectionProjectServices : ISectionProjectServices
    {
        private readonly ISectionProjectRepository _sectionProjectRepository;

        public SectionProjectServices()
        {
            _sectionProjectRepository = new SectionProjectRepository();
        }

        public SectionProject GetInfo(long sectionprojectId)
        {
            var sectionProject = _sectionProjectRepository.Get(sectionprojectId);
            if (sectionProject == null)
                throw new NotFoundException("SectionProject not found");
            return sectionProject;
        }



        public void Approve(long sectionprojectId)
        {
            var rel = _sectionProjectRepository.Get(sectionprojectId);
            if (rel == null)
                throw new NotFoundException("SectionProject not found");
            rel.IsApproved = true;
            _sectionProjectRepository.Update(rel);
            _sectionProjectRepository.Save();
        }

        public IQueryable<SectionProject> GetUnapproved()
        {
            return _sectionProjectRepository.GetUnapprovedProjects();

        }

        public SectionProject AddOrUpdate(SectionProjectEntryModel sectionProjectEntryModel)
        {
            var sectionproject = _sectionProjectRepository.GetSectionProjectByIds(sectionProjectEntryModel.SectiontId,
                sectionProjectEntryModel.ProjectId);
            if(sectionproject==null)
                throw new NotFoundException("SectionProject not found");
            sectionproject.Description = sectionProjectEntryModel.Description;
            sectionproject.Cost=sectionProjectEntryModel.Cost;
            _sectionProjectRepository.Update(sectionproject);
            _sectionProjectRepository.Save();
            return sectionproject;
        }
    }
}