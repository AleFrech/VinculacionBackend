using System;

namespace VinculacionBackend.Entities
{
    public class Student
    {

        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string Major { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
        public Status Status { get; set; }
    }
}