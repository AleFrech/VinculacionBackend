using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private readonly VinculacionContext _db;

		public UserRepository()
		{
			_db = new VinculacionContext();
		}
		public Section Delete(long id)
		{
			var found = Get(id);
			_db.Users.Remove(found);
			return found;
		}

		public Section Get(long id)
		{
			return _db.Users.Include(a => a.Major).Include(b => b.Section).FirstOrDefault(d=>d.Id==id);
		}

		public IQueryable<Section> GetAll()
		{
			return _db.Users.Include(a => a.Major).Include(b => b.Section);
		}

		public void Insert(User ent)
		{
			_db.Users.Add(ent);
		}

		public void Save()
		{
			_db.SaveChanges();
		}

		public void Update(User ent)
		{
			_db.Entry(ent).State = EntityState.Modified;
		}
		
		public IEnumerable<User> GetUserByEmailAndPassword(string email, string password)
		{
			return _db.Users.Include(a => a.Major).Include(b => b.Section).FirstOrDefault(d=>d.Email == email && d.Password == password);
		}
	}
}