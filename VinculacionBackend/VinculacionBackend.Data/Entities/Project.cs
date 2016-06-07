using System.Collections.Generic;
using System.Security.Policy;

namespace VinculacionBackend.Data.Entities
{
    public class Project
    {
        public long Id { get; set; }
        public string ProjectId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Cost { get; set; }
        public List<string> MajorIds { get; set; }
        public List<long> SectionIds { get; set; }
        public bool IsDeleted {get; set;}
        public string BeneficiarieOrganization{get;set;}
        public string BeneficiarieGroups { get; set; }
        public int BeneficiariesQuantity{get;set;}
    }
}