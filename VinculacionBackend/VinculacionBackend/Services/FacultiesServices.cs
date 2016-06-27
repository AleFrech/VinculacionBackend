﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.OData.Query;
using Newtonsoft.Json;
using Spire.Pdf.Exporting.XPS.Schema;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Data.Models;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Services
{
    public class PeriodCostModel
    {
        public int PeriodNumber { get; set; }
        public float Cost { get; set; }
    }
    public class FacultiesServices : IFacultiesServices
    {
        readonly IFacultyRepository _facultyRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IStudentRepository _studentRepository;
        private List<int> Periods ;
        public FacultiesServices(IFacultyRepository facultyRepository, IMajorRepository majorRepository, IProjectRepository projectRepository, IStudentRepository studentRepository)
        {
            _facultyRepository = facultyRepository;
            _majorRepository = majorRepository;
            _projectRepository = projectRepository;
            _studentRepository = studentRepository;
            Periods = new List<int>();
            Periods.Add(1);
            Periods.Add(2);
            Periods.Add(3);
            Periods.Add(5);

        }

       
        public Dictionary<string, List<PeriodCostModel>> GetFacultiesCosts(int i,int year)
        {
            Dictionary<string, List<PeriodCostModel>> facultiesCosts = new Dictionary<string, List<PeriodCostModel>>();
            List<PeriodCostModel> periodCosts = new List<PeriodCostModel>();
            var facultyCosts = new List<FacultyProjectCostModel>();
            var totalCosts = 0.0f;

                periodCosts.Add(new PeriodCostModel { PeriodNumber = 0, Cost = totalCosts });
                periodCosts.Add(new PeriodCostModel { PeriodNumber = 0, Cost = totalCosts });
                periodCosts.Add(new PeriodCostModel { PeriodNumber = 0, Cost = totalCosts });
                periodCosts.Add(new PeriodCostModel { PeriodNumber = 0, Cost = totalCosts });

            for (var j = 0; j < 4; j++)
                {
                    facultyCosts = _facultyRepository.GetFacultyCosts(i, Periods.ElementAt(j), year);
                    if (facultyCosts.Count > 0)
                    {
                        totalCosts = facultyCosts.Sum(fc => (float) fc.ProjectCost);
                        periodCosts.ElementAt(j).PeriodNumber = Periods.ElementAt(j);
                        periodCosts.ElementAt(j).Cost = totalCosts;
                    }
                }
                if (facultyCosts.Count > 0)
                    facultiesCosts.Add(facultyCosts.ElementAt(0).FacultyName, periodCosts);
 

            return facultiesCosts;
        }

        public Dictionary<string, int> GetFacultiesHours()
        {
            Dictionary<string, int> facultiesHours = new Dictionary<string, int>();
            var faculties = _facultyRepository.GetAll();
            foreach (var faculty in faculties.ToList())
            {
                facultiesHours[faculty.Name] = 0;

                var facultyMajors = _majorRepository.GetMajorsByFaculty(faculty.Id);
                foreach (var major in facultyMajors)
                {
                    var facultyProjects = _projectRepository.GetByMajor(major.MajorId);
                    foreach (var project in facultyProjects)
                    {
                        var projectHours = _studentRepository.GetStudentsHoursByProject(project.Id);
                    //    facultiesHours[faculty.Name] += projectHours;
                    }
                }
            }

            return facultiesHours;
        }
        public DataTable CreateFacultiesCostReport()
        {
            var dt = new DataTable();
            dt.Columns.Add("Facultad", typeof(string));
            dt.Columns.Add("Periodo 1", typeof(float));    
            dt.Columns.Add("Periodo 2", typeof(float));
            dt.Columns.Add("Periodo 3", typeof(float));
            dt.Columns.Add("Periodo 5", typeof(float));

            var FacultyCosts = GetFacultiesCosts(1,2013);
            foreach (var key in FacultyCosts.Keys)
            {
                dt.Rows.Add(key, FacultyCosts[key].ElementAt(0).Cost, FacultyCosts[key].ElementAt(1).Cost
                    , FacultyCosts[key].ElementAt(2).Cost, FacultyCosts[key].ElementAt(3).Cost);
            }

             FacultyCosts = GetFacultiesCosts(2, 2013);
            foreach (var key in FacultyCosts.Keys)
            {
                dt.Rows.Add(key, FacultyCosts[key].ElementAt(0).Cost, FacultyCosts[key].ElementAt(1).Cost
                    , FacultyCosts[key].ElementAt(2).Cost, FacultyCosts[key].ElementAt(3).Cost);
            }
            return dt;
        }
    }
}