namespace VinculacionBackend.Data.Entities
{
    public class Section
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public Class Class { get; set; }
        public Period Period { get; set; }
        public User User { get; set; }
    }
}