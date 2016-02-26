namespace VinculacionBackend.Entities
{
    public class Hour
    {
        public long Id { get; set; }
        public int Amount { get; set; }
        public SectionProyect SectionProyect { get; set; }
        public User User { get; set; }

    }
}