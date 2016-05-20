using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Cors;
using System.Web.OData;
using VinculacionBackend.Cache;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class MajorsController : ApiController
    {
        private readonly IMajorsServices _majorsServices;
        private readonly MemoryCacher _memCacher;
        public MajorsController(IMajorsServices majorsServices)
        {
            _majorsServices = majorsServices;
            _memCacher = new MemoryCacher();
        }

        // GET: api/Majors
        [Route("api/Majors")]
        [EnableQuery]
        public IEnumerable<Major> GetMajors()
        {
            //var result = _memCacher.GetValue("majors");
            //if (result == null)
            //{
            //    _memCacher.Add("majors", _majorsServices.All().ToList(), DateTimeOffset.UtcNow.AddHours(24));
            //    result = _memCacher.GetValue("majors");
            //}
            //return result as IEnumerable<Major>;


            Major[] majors =
            {
                new Major {MajorId = "I - 02", Name = "INGENIERÍA INDUSTRIAL Y DE SISTEMAS"},
                new Major {MajorId = "I - 03", Name = "INGENIERÍA CIVIL"},
                new Major {MajorId = "I - 04", Name = "INGENIERÍA EN TELECOMUNICACIONES"},
                new Major {MajorId = "I - 05", Name = "INGENIERÍA EN MECATRÓNICANICA"},
                new Major {MajorId = "I - 06", Name = "INGENIERÍA EN INFORMÁTICA"},
                new Major {MajorId = "I - 07", Name = "INGENIERÍA EN SISTEMAS ELECTRÓNICOS"},
                new Major {MajorId = "I - 09", Name = "INGENIERÍA EN GESTIÓN LOGÍSTICA"},
                new Major {MajorId = "I - 10", Name = "INGENIERÍA EN BIOMÉDICA"},
                new Major {MajorId = "I - 11", Name = "ARQUITECTURA"},
                new Major {MajorId = "I - 12", Name = "INGENIERÍA EN ENERGÍA"},
                new Major {MajorId = "L - 02", Name = "LICENCIATURA EN ADMINISTRACIÓN INDUSTRIAL Y DE NEGOCIOS"},
                new Major {MajorId = "L - 04", Name = "LICENCIATURA EN FINANZAS"},
                new Major {MajorId = "L - 06", Name = "LICENCIATURA EN MERCADOTECNIA(PROMOCIÓN Y PUBLICIDAD)"},
                new Major {MajorId = "L - 07", Name = "LICENCIATURA EN COMUNICACIÓN Y PUBLICIDAD"},
                new Major {MajorId = "L - 08", Name = "LICENCIATURA EN MERCADOTECNIA Y NEGOCIOS INTERNACIONALES"},
                new Major {MajorId = "L - 09", Name = "LICENCIATURA EN ADMINISTRACIÓN DE EMPRESAS TURÍSTICAS"},
                new Major {MajorId = "L - 10", Name = "LICENCIATURA EN DERECHO"},
                new Major {MajorId = "L - 12", Name = "LICENCIATURA EN DISEÑO GRAFICO"},
                new Major {MajorId = "L - 13", Name = "LICENCIATURA EN RELACIONES INTERNACIONALES"},
                new Major {MajorId = "L - 14", Name = "LICENCIATURA EN PSICOLOGÍA CON ORIENTACIÓN EMPRESARIAL"},
                new Major {MajorId = "L - 15", Name = "LICENCIATURA EN CONTADURÍA PÚBLICA"},
                new Major {MajorId = "L - 16", Name = "LICENCIATURA EN ADMINISTRACIÓN DE EMPRESAS(CEUTEC)"},
                new Major {MajorId = "T - 03", Name = "TÉCNICO UNIVERSITARIO EN MERCADOTECNIA Y VENTAS(CEUTEC)"},
                new Major {MajorId = "T - 05", Name = "TÉCNICO UNIVERSITARIO EN ADMINISTRACIÓN(CEUTEC)"},
                new Major {MajorId = "T - 07", Name = "TÉCNICO EN DESARROLLO DE SISTEMAS DE INFORMACIÓN(CEUTEC)"},
                new Major {MajorId = "T - 08", Name = "TÉCNICO UNIVERSITARIO EN DISEÑO GRÁFICO(CEUTEC)"}
            };

            return majors;
        }
      
    

        // GET: api/Majors/5
        [ResponseType(typeof(Major))]
        [Route("api/Majors/{majorId}")]
        public IHttpActionResult GetMajor(string majorId)
        {
            Major major = _majorsServices.Find(majorId);
            return Ok(major);
        }   
    }
}