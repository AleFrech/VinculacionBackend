﻿using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using VinculacionBackend.Data.Database;
using VinculacionBackend.Data.Repositories;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Services;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]


    public class ReportsController : ApiController
    {
        private readonly IFacultiesServices _facultiesServices;
        private readonly ISheetsReportsServices _reportsServices;

        public ReportsController(IFacultiesServices facultiesServices, ISheetsReportsServices reportsServices)
        {
            _facultiesServices = facultiesServices;
            _reportsServices = reportsServices;
        }


        [Route("api/Reports/CostsReport")]
        public IHttpActionResult GetCostsReport()
        {
            var context = _reportsServices.GenerateReport(_facultiesServices.CreateFacultiesCostReport(),
                "Reporte de Costos por Facultad");
            context.Response.Flush();
            context.Response.End();
            return Ok();
        }
    
    }
}