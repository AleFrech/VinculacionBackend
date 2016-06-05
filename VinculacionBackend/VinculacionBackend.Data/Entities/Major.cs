namespace VinculacionBackend.Data.Entities
{
    public class Major
    {
        public long Id { get; set; }
        public string MajorId { get; set; }
        public string Name { get; set; }     
        public Faculty Faculty { get; set; }  
    }
}