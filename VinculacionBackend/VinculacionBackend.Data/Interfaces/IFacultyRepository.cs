using System.Collections.Generic;
using System.Linq;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Models;
using VinculacionBackend.Data.Repositories;


namespace VinculacionBackend.Data.Interfaces
{
    public interface IFacultyRepository { 

        List<FacultyProjectCostModel> GetFacultyCosts(int id,int period, int year);
        IQueryable<Faculty> GetAll();
    }
}