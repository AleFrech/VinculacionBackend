using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Entities
{
    public class User_Role
    {
        public long Id { get; set; }
        public User User { get; set; }
        public Role Role { get; set; }

    }
}