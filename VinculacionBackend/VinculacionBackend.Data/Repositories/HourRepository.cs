using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Exceptions;


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
            return _db.Hours.Find(id);
        }

        public IQueryable<Hour> GetAll()
        {
            return _db.Hours;
        }

        public IQueryable<Hour> GetStudentHours(string accountId)
        {
            return _db.Hours.Where(hour => hour.User.AccountId == accountId)
                .Include("SectionProject.Project").Include("SectionProject.Section").Include("SectionProject.Section.Period");
        }     

        public void Insert(Hour ent)
        {
            _db.Hours.Add(ent);
        }

        public Hour InsertHourFromModel(string accountId,long sectionId,long projectId, int hour,string professorUser )
        {
            var sectionProjectRel = Queryable.FirstOrDefault(_db.SectionProjectsRels.Include(x => x.Project).Include(y => y.Section), z => z.Section.Id == sectionId && z.Project.Id == projectId);
            var user = Queryable.FirstOrDefault(_db.Users, x => x.AccountId == accountId);
            var section = Queryable.FirstOrDefault(_db.Sections.Include(x=>x.User).Include(x=>x.Class).Include(x=>x.Period), x => x.Id == sectionId);
            if(user==null)
                throw new NotFoundException("No se encontro el estudiante");
            if(section==null)
                throw new NotFoundException("No se encontro la seccion");
            if(sectionProjectRel==null)
                throw new NotFoundException("No se encontro el proyecto");
            
                if(section.User.Email!=professorUser)
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
    }
}
