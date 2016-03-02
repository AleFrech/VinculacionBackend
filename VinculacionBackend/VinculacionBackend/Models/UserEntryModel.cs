using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Models
{
    public class UserEntryModel
    {
        public string AccountId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string MajorId { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
    }
}