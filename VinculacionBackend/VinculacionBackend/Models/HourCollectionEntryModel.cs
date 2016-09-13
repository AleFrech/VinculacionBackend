using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VinculacionBackend.Models
{
    public class HourCollectionEntryModel
    {
        [Required(ErrorMessage = "*requerido")]
        public long SectionId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public long ProjectId { get; set; }
        [Required(ErrorMessage = "*requerido")]
        public List<StudentHourEntry> StudentsHour { get; set; }

    }

    public class StudentHourEntry
    {
        public string AccountId { get; set; }
        public long HourId { get; set; }
        public int Hour { get; set; }

    }
}