using System.ComponentModel.DataAnnotations;
using System.Linq;
using VinculacionBackend.Data.Database;

namespace VinculacionBackend.CustomDataNotations
{
    public class AccountNumberExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var accountNumber = value.ToString();
            var context = new VinculacionContext();
            return Enumerable.All(context.Users, u => !accountNumber.Equals(u.AccountId));

        }
    }
}