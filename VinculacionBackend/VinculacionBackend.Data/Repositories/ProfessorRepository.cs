using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
	public class ProfessorRepository : IProfessorRepository
	{
		private readonly VinculacionContext _db;
		
		public ProfessorRepository() 
		{
			_db = new VinculacionContext();
		}
		
		public IQueryable<User> GetAll()
		{
			var rels = GetUserRoleRelationships();
			var professors = _db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id));
			return professors;
		}
		
		public User Get(long id)
		{
			var rels = GetUserRoleRelationships();
			var professor = _db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.Id == id);
			return professor;
		}

        public User GetByAccountId(string accountId)
        {
            var rels = GetUserRoleRelationships();
            var professor = _db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.AccountId == accountId);
            return professor;
        }

	    public User DeleteByAccountNumber(string accountId)
	    {
            var found = GetByAccountId(accountId);
            if (found != null)
            {
                var userrole = _db.UserRoleRels.FirstOrDefault(x => x.User.AccountId == found.AccountId);
                _db.UserRoleRels.Remove(userrole);
                _db.Users.Remove(found);
            }
            return found;
        }

	    public User Delete(long id)
		{
			var found = Get(id);
			_db.Users.Remove(found);
			return found;
		}
		
		public void Save()
		{
		    _db.SaveChanges();
		}
		
		public void Update(User ent)
		{
		    _db.Entry(ent).State = EntityState.Modified;
		}
		
		public void Insert(User ent)
		{
			_db.Users.Add(ent);
			_db.UserRoleRels.Add(new UserRole { User=ent,Role=_db.Roles.FirstOrDefault(x=>x.Name=="Professor")});
		}

	    private IEnumerable<UserRole> GetUserRoleRelationships()
		{
		    return _db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Professor");
		}
		
	}
}
