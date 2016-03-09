using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VinculacionBackend.CostumeDataNotations
{
    public class AccountNumberExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var accountNumber = value.ToString();
            var context = new Database.VinculacionContext();
            return Enumerable.All(context.Users, u => !u.AccountId.Equals(accountNumber));

        }
    }
}