namespace VinculacionBackend.Data.Entities
{
    public class MajorUser
    {
        public long Id { get; set; }
        public Major Major { get; set; }
        public User User { get; set; }
    }
}