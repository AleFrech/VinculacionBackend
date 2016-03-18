using System.ComponentModel.DataAnnotations;
using System.Linq;
using VinculacionBackend.Entities;

namespace VinculacionBackend.CostumeDataNotations
{
    public class EmailExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var email = value.ToString();
            var context = new Database.VinculacionContext();
            return Enumerable.All(context.Users, u => !u.Email.Equals(email));
        }
    }
}
