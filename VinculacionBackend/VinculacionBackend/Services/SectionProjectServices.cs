using System;
using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Exceptions;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Repositories;
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
                throw new InvalidSectionOrProjectException("Seccion o proyecto invalido");
            return sectionProject;
        }



        public void Approve(long sectionprojectId)
        {
            var rel = _sectionProjectRepository.Get(sectionprojectId);
            if (rel == null)
                throw new InvalidSectionOrProjectException("Seccion o proyecto invalido");
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

            foreach (var projectId in sectionProjectEntryModel.ProjectIds)
            {
            var sectionproject = _sectionProjectRepository.GetSectionProjectByIds(sectionProjectEntryModel.SectiontId,
                projectId);
                if (sectionproject == null)
                {
                    var project = _projectRepository.Get(projectId);
                    var section = _sectionRepository.Get(sectionProjectEntryModel.SectiontId);

                    if (project == null || section == null)
                    {
                        throw new InvalidSectionOrProjectException("proyecto o seccion no valido");
                    }
    
                  sectionproject = new SectionProject {
                      Section = section,
                      Project = project,
                      Description = sectionProjectEntryModel.Description,
                      Cost=sectionProjectEntryModel.Cost,
                      IsApproved = false
                   };
                  _sectionProjectRepository.Insert(sectionproject);
                  _sectionProjectRepository.Save();
                }
                sectionproject.Description = sectionProjectEntryModel.Description;
                sectionproject.Cost = sectionProjectEntryModel.Cost;
                _sectionProjectRepository.Update(sectionproject);
                sectionProjects.Add(sectionproject);
                _sectionProjectRepository.Save();
            }

            _sectionProjectRepository.Save();
            return sectionProjects;
        }

        public SectionProject GetInfo(long sectionId, long projectId)
        {
            var sectionProject = _sectionProjectRepository.GetSectionProjectByIds(sectionId, projectId);
            if (sectionProject == null)
                throw new InvalidSectionOrProjectException("Seccion o proyecto invalido");
            return sectionProject;
        }
    }
}