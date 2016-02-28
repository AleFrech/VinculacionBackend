using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using VinculacionBackend.Database;
using VinculacionBackend.Entities;

namespace VinculacionBackend.Security
{
    public class CredentialChecker
    {
        VinculacionContext db = new VinculacionContext();
        public User CheckCredential(string email, string password)
        {
            var rels = db.UserRoleRels.Include(x => x.Role).Include(y => y.User).Where(z => z.Role.Name == "Professor");
            var student = db.Users.FirstOrDefault(x => rels.Any(y => y.User.Id == x.Id));
            return new User();
        }
    }
}