using System;
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
		public User Delete(long id)
		{
			var found = Get(id);
			_db.Users.Remove(found);
			return found;
		}

		public User Get(long id)
		{
			return _db.Users.FirstOrDefault(d=>d.Id==id);
		}

		public IQueryable<User> GetAll()
		{
			return _db.Users;
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
		
		public User GetUserByEmailAndPassword(string email, string password)
		{
			return _db.Users.FirstOrDefault(d=>d.Email == email && d.Password == password);
		}
		
		public Role GetUserRole(string email)
		{
			return _db.UserRoleRels.Include(x=>x.User).Include(y=>y.Role).FirstOrDefault(z=>z.User.Email == email)?.Role;
		}

        public bool isAdmin(string userEmail)
        {
            return _db.UserRoleRels.Where(a => a.User.Email == userEmail && a.Role.Name.Equals("Admin")).Any();
        }
    }
}