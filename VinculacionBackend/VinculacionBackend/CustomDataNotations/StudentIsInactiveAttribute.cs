using System.ComponentModel.DataAnnotations;
using System.Linq;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Enums;

namespace VinculacionBackend.CustomDataNotations
{
    public class StudentIsInactiveAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var accountNumber = value.ToString();
            var context = new VinculacionContext();
            var student = context.Users.FirstOrDefault(x => x.AccountId == accountNumber);
            if(student!=null)
                return student.Status == Status.Inactive;
            return false;
        }
    }
}