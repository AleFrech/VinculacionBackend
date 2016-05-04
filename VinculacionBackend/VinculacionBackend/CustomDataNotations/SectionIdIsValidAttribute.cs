using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using VinculacionBackend.Data.Repositories;

namespace VinculacionBackend.CustomDataNotations
{
    public class SectionIdIsValidAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            var id = (long)value;
            var rep = new SectionRepository();
            return rep.Get(id) != null;
        }
    }
}