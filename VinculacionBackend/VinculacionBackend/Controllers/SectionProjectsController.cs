﻿using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using VinculacionBackend.ActionFilters;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;
using VinculacionBackend.Security.BasicAuthentication;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SectionProjectsController : ApiController
    {
        private readonly ISectionProjectServices _sectionProjectServices;

        public SectionProjectsController(ISectionProjectServices sectionProjectServices)
        {
            _sectionProjectServices = sectionProjectServices;
        }


        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects/Info/{sectionId}/{projectId}")]
        [CustomAuthorize(Roles = "Admin,Professor")]
        public IHttpActionResult GetSectionProject(long sectionId,long projectId)
        {
            return Ok(_sectionProjectServices.GetInfo(sectionId,projectId));
        }

        // POST: api/SectionProjects
        [ResponseType(typeof(SectionProject))]
        [Route("api/SectionProjects")]
        [CustomAuthorize(Roles = "Admin")]
        [ValidateModel]
        public IHttpActionResult PostSectionProject(SectionProjectEntryModel sectionProjectEntryModel)
        {
             _sectionProjectServices.AddOrUpdate(sectionProjectEntryModel);
            return Ok();
        }

        [ResponseType(typeof(void))]
        [Route("api/SectionProjects/Approve/{sectionId}/{projectId}")]
        [CustomAuthorize(Roles = "Admin")]
        public IHttpActionResult PutSectionProject(long sectionId, long projectId)
        {
            _sectionProjectServices.Approve(sectionId,projectId);
            return Ok();
        }

    }
}