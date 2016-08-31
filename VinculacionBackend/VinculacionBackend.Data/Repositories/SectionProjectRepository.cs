using System.Data.Entity;
﻿using System;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
    public class SectionProjectRepository : ISectionProjectRepository
    {
        private readonly VinculacionContext _db;

        public SectionProjectRepository()
        {
            _db = new VinculacionContext();
        }

        public IQueryable<SectionProject> GetAll()
        {
            return _db.SectionProjectsRels.Include(rel => rel.Section).Include(rel => rel.Project);
        }

        public SectionProject Get(long id)
        {
            return
                _db.SectionProjectsRels.Include(rel => rel.Section)
                    .Include(rel => rel.Project)
                    .Include(rel => rel.Section.User)
                    .Include(rel => rel.Section.Class)
                    .FirstOrDefault(rel => rel.Id == id);
        }



        public SectionProject GetSectionProjectByIds(long sectionId, long projectId)
        {
            return
                _db.SectionProjectsRels.Include(rel => rel.Section)
                    .Include(rel => rel.Project)
                    .Include(rel => rel.Section.User)
                    .Include(rel => rel.Section.Class)
                    .FirstOrDefault(rel => rel.Project.Id == projectId && rel.Section.Id==sectionId);
        }

        public SectionProject Delete(long id)
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(SectionProject ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
        }


        public void Insert(SectionProject ent)
        {
            _db.SectionProjectsRels.Add(ent);
        }
        
        public IQueryable<SectionProject> GetUnapprovedProjects()
        {
            return _db.Hours.Include(hour => hour.SectionProject)
                .Where(hour => !hour.SectionProject.IsApproved)
                .Select(hour => hour.SectionProject)
                .Include(rel => rel.Section)
                .Include(rel => rel.Project)
                .Include(rel => rel.Section.Class)
                .Include(rel => rel.Section.User)
                .Include(rel => rel.Section.Period)
                .Distinct();
        }
    }
}
