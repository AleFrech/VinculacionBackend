using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using VinculacionBackend.Enums;

namespace VinculacionBackend.Entities
{
    public class Student
    {
        public long Id { get; set;  }
        public string IdNumber { get; set; }
        public string Name { get; set; }
        public string Major { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
        public Status Status { get; set; }
    }
}