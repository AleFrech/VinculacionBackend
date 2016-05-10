using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Data.Interfaces
{
	public interface IProfessorRepository : IRepository<User>
	{
	    User GetByAccountId(string accountId);
        User DeleteByAccountNumber(string accountNumber);
    }
}
