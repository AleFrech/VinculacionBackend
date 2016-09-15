using System.Collections.Generic;
using VinculacionBackend.Data.Entities;

namespace VinculacionBackend.Models
{
    public class ProjectFinalReportModel
    {
        public Project Project { get; set; }
        public Section Section { get; set; }
        public List<User> StudentsInSections { get; set; }
        public string MajorsOfStudents { get; set; }
        public SectionProject SectionProject { get; set; }
        public int FieldHours { get; set; }
        public int Calification { get; set; }
        public int BeneficiariesQuantity { get; set; }
        public string BeneficiarieGroups { get; set; }
        public Dictionary<User, int> StudentsHours { get; set; }
        public string ProfessorName { get; set; }


    }
}