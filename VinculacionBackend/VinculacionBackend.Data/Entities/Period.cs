namespace VinculacionBackend.Data.Entities
{
    public class Period
    {
        public long Id { get; set; }
        public int Number { get; set; }
        public int Year { get; set; }
        public bool IsCurrent { get; set; }
    }
}