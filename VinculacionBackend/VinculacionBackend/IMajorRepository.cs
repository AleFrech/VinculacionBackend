﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VinculacionBackend.Entities;

namespace VinculacionBackend
{
    interface IMajorRepository : IRepository<Major>
    {
        Major GetMajorByMajorId(string majorId);
    }
}