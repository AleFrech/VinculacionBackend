namespace VinculacionBackend.Data.Entities
{
    public class SectionProject
    {
        public long Id { get; set; }
        public Section Section { get; set; }
        public Project Project { get; set; }
        public bool IsApproved { get; set; }
        public string Description { get; set;}
        public double Cost { get; set; }
    }
}
