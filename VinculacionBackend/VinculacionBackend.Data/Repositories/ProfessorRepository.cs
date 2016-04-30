using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Enums;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
	public class ProfessorRepository : IProfessorRepository
	{
		private VinculacionContext db;
		
		public ProfessorRepository() 
		{
			db = new VinculacionContext();
		}
		
		public IQueryable<User> GetAll()
		{
			var rels = GetUserRoleRelationships();
			var professors = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id));
			return professors;
		}
		
		public User Get(long id)
		{
			var rels = GetUserRoleRelationships();
			var professor = db.Users.Include(m => m.Major).Where(x => rels.Any(y => y.User.Id == x.Id)).FirstOrDefault(z => z.Id == id);
			return professor;
		}
		
		public User Delete(long id)
		{
			var found = Get(id);
			db.Users.Remove(found);
			return found;
		}
		
		public void Save()
		{
		    db.SaveChanges();
		}
		
		public void Update(User ent)
		{
		    db.Entry(ent).State = EntityState.Modified;
		}
		
		public void Insert(User ent)
		{
			db.Users.Add(ent);
			db.UserRoleRels.Add(new UserRole { User=ent,Role=db.Roles.FirstOrDefault(x=>x.Name=="Professor")});
		}
		
		private IEnumerable<UserRole> GetUserRoleRelationships()
		{
		    return db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Professor");
		}
		
	}
}
