using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VinculacionBackend.Entities
{
    public class Class
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }

    public class Hour
    {
        public long Id { get; set; }
        public int Amount { get; set; }
        public SectionProyect SectionProyect { get; set; }
        public User User { get; set; }

    }




}