using System.Collections.Generic;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
	public interface IUserRepository : IRepository<User>
	{
		User GetUserByEmailAndPassword(string email, string password);
	}
}