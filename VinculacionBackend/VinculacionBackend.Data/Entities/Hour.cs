namespace VinculacionBackend.Data.Entities
{
    public class Hour
    {
        public long Id { get; set; }
        public int Amount { get; set; }
        public SectionProject SectionProject { get; set; }
        public User User { get; set; }

    }
}