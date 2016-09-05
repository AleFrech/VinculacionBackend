using System.Data.Entity;
using System.Linq;
using Castle.Components.DictionaryAdapter.Xml;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Exceptions;

namespace VinculacionBackend.Data.Repositories
{
    public class HourRepository : IHourRepository
    {
        private readonly VinculacionContext _db;
        public HourRepository()
        {
            _db = new VinculacionContext();
        }
        public Hour Delete(long id)
        {
            var found = Get(id);
            _db.Hours.Remove(found);
            return found;
        }

        public Hour Get(long id)
        {
            return _db.Hours.Include("User").Include("SectionProject").FirstOrDefault(x=>x.Id==id);
        }

        public IQueryable<Hour> GetAll()
        {
            return _db.Hours.Include("User").Include("SectionProject").Include(x=>x.SectionProject.Section).Include(x => x.SectionProject.Section.Period);
        }

        public IQueryable<Hour> GetStudentHours(string accountId)
        {
            return _db.Hours.Where(hour => hour.User.AccountId == accountId)
                .Include("SectionProject.Project").Include("SectionProject.Section").Include("SectionProject.Section.Period");
        }

        public SectionProject GetSectionProjectRel(long sectionProjectId)
        {
            return
                _db.SectionProjectsRels.Include(rel => rel.Section)
                    .Include(rel => rel.Project)
                    .FirstOrDefault(rel => rel.Id == sectionProjectId);
        }

        public void Insert(Hour ent)
        {
            _db.Hours.Add(ent);
        }

        public Hour InsertHourFromModel(string accountId,long sectionId,long projectId, int hour,string professorUser, bool isAdmin )
        {
            var sectionProjectRel = Queryable.FirstOrDefault(_db.SectionProjectsRels.Include(x => x.Project).Include(y => y.Section), z => z.Section.Id == sectionId && z.Project.Id == projectId);
            if (sectionProjectRel.IsApproved)
                throw new HoursAlreadyApprovedException("Las Horas no se pueden modificar porque ya han sido aprobadas");
            var user = Queryable.FirstOrDefault(_db.Users, x => x.AccountId == accountId);
            var section = Queryable.FirstOrDefault(_db.Sections.Include(x=>x.User).Include(x=>x.Class).Include(x=>x.Period), x => x.Id == sectionId);
            if(user==null)
                throw new NotFoundException("No se encontro el estudiante");
            if(section==null)
                throw new NotFoundException("No se encontro la seccion");
            if(sectionProjectRel==null)
                throw new NotFoundException("No se encontro el proyecto");
            
                if(section.User.Email!=professorUser && !isAdmin)
                    throw new UnauthorizedException("No tiene permisos para agregar horas a este proyecto");
                var Hour = new Hour();
                Hour.Amount = hour;
                Hour.SectionProject = sectionProjectRel;
                Hour.User = user;
                Insert(Hour);
                return Hour;
           
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public void Update(Hour ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
        }

        public void Update(SectionProject ent)
        {
            _db.Entry(ent).State = EntityState.Modified;
            
        }
    }
}
