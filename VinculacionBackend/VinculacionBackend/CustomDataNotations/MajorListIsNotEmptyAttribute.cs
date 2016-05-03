using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.CustomDataNotations
{
    public class MajorListIsNotEmptyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var list = (List<string>)value;

            return list.Count > 0;
        }
    }
}