using System;

namespace VinculacionBackend.Entity
{
    public class Student
    {
        public Student()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string IdNumber { get; set; }
        public string Major { get; set; }
        public string Campus { get; set; }
        public string Email { get; set; }
        public Status Status { get; set; }
    }
}