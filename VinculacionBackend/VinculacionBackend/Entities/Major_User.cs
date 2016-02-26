using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Entities
{
    public class Major_User
    {
        public long Id { get; set; }
        public Major Major { get; set; }
        public User User { get; set; }
    }
}