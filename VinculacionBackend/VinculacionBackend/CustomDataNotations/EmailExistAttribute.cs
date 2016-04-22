using System.ComponentModel.DataAnnotations;
using System.Linq;
using VinculacionBackend.Data.Database;

namespace VinculacionBackend.CustomDataNotations
{
    public class EmailExistAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var email = value.ToString();
            var context = new VinculacionContext();
            return Enumerable.All(context.Users, u => !email.Equals(u.Email));
        }
    }
}
