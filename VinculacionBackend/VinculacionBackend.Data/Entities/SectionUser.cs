namespace VinculacionBackend.Data.Entities
{
    public class SectionUser
    {
        public long Id { get; set; }
        public Section Section { get; set; }
        public User User { get; set; }

    }
}