using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            if(sectionProject == null)
                throw new NotFoundException(id + " not found");
            return new SectionProjectInfoModel
            {
                ClassName = sectionProject.Section.Class.Name,
                ProfessorName = sectionProject.Section.User.Name,
                ProjectName = sectionProject.Project.Name
            };
        }
    }
}