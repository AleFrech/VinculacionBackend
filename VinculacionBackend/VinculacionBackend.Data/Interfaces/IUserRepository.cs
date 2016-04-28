using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
	interface IUserRepository : IRepository<User>
	{
		IEnumerable<User> GetUserByEmailAndPassword(string email, string password);
	}
}