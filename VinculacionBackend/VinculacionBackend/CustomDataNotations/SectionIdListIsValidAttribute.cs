using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VinculacionBackend.Data.Repositories;

namespace VinculacionBackend.CustomDataNotations
{
    public class SectionIdListIsValidAttribute : ValidationAttribute
    {
      
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var list = (List<long>)value;

            return list.Count > 0;
        }
    }
}