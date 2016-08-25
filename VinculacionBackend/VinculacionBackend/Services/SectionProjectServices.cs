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

        public SectionProjectInfoModel GetInfo(long id)
        {
            var sectionProject = _sectionProjectRepository.Get(id);
            if (sectionProject == null)
                throw new NotFoundException(id + " not found");
            return new SectionProjectInfoModel
            {
                ClassName = sectionProject.Section.Class.Name,
                ProfessorName = sectionProject.Section.User.Name,
                Project = sectionProject.Project
            };
        }

        public void Approve(long sectionProjectId)
        {
            var rel = _sectionProjectRepository.Get(sectionProjectId);
            if (rel == null)
                throw new NotFoundException(sectionProjectId + " not found");

            rel.IsApproved = true;
            _sectionProjectRepository.Update(rel);
            _sectionProjectRepository.Save();
        }

        public IQueryable<SectionProject> GetUnapproved()
        {
            return _sectionProjectRepository.GetUnapprovedProjects();
        }
    }
}