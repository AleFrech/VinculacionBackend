using System.Data.Entity;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;

namespace VinculacionBackend.Data.Repositories
{
	public class UserRepository : IUserRepository
	{
		private VinculacionContext db;

		public UserRepository()
		{
			db = new VinculacionContext();
		}
		public Section Delete(long id)
		{
			var found = Get(id);
			db.Users.Remove(found);
			return found;
		}

		public Section Get(long id)
		{
			return db.Users.Include(a => a.Major).Include(b => b.Section).FirstOrDefault(d=>d.Id==id);
		}

		public IQueryable<Section> GetAll()
		{
			return db.Users.Include(a => a.Major).Include(b => b.Section);
		}

		public void Insert(User ent)
		{
			db.Users.Add(ent);
		}

		public void Save()
		{
			db.SaveChanges();
		}

		public void Update(User ent)
		{
			db.Entry(ent).State = EntityState.Modified;
		}
		
		public IEnumerable<User> GetUserByEmailAndPassword(string email, string password)
		{
			return db.Users.Include(a => a.Major).Include(b => b.Section).FirstOrDefault(d=>d.Email == email && d.Password == password);
		}
	}
}