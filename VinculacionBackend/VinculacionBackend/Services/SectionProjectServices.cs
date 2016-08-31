using System.Collections.Generic;
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
        private readonly ISectionRepository _sectionRepository;
        private readonly IProjectRepository _projectRepository;

        public SectionProjectServices()
        {
            _sectionProjectRepository = new SectionProjectRepository();
            _sectionRepository = new SectionRepository();
            _projectRepository = new ProjectRepository();
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

        public IList<SectionProject> AddOrUpdate(SectionProjectEntryModel sectionProjectEntryModel)
        {
            IList<SectionProject> sectionProjects = new List<SectionProject>();

            foreach (var ProjectId in sectionProjectEntryModel.ProjectIds)
            {
            var sectionproject = _sectionProjectRepository.GetSectionProjectByIds(sectionProjectEntryModel.SectiontId,
                ProjectId);
                if (sectionproject == null)
                {
                    var project = _projectRepository.Get(ProjectId);
                    var section = _sectionRepository.Get(sectionProjectEntryModel.SectiontId);

                    if (project == null)
                    {
                        throw new NotFoundException("project not found");
                    }
                    if (section == null)
                    {
                        throw new NotFoundException("section not found");
                    }
                  sectionproject = new SectionProject {
                      Section = section,
                      Project = project,
                      IsApproved = false,
                   };
                  _sectionProjectRepository.Insert(sectionproject);
                }
            sectionproject.Description = sectionProjectEntryModel.Description;
                sectionproject.Cost = sectionProjectEntryModel.Cost;
            _sectionProjectRepository.Update(sectionproject);

                sectionProjects.Add(sectionproject);
            }

            _sectionProjectRepository.Save();
            return sectionProjects;
        }
    }
}