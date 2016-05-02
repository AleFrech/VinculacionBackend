namespace VinculacionBackend.Data.Entities
{
        public class ProjectMajor
        {
            public long Id { get; set; }
            public Project Project { get; set; }
            public Major Major { get; set; }
        }
    
}
