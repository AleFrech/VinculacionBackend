using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VinculacionBackend.Tests
{
    public class Reporte
    {
        private readonly IAlumnoRepository _alumnoRepository;
        private readonly IMailManager _mailManager;

        public Reporte(IAlumnoRepository alumnoRepository, IMailManager mailManager)
        {
            _alumnoRepository = alumnoRepository;
            _mailManager = mailManager;
        }


        public double ObtenerPromedio()
        {
            var promedio = 0D;
            var alumnos = _alumnoRepository.GetAlumnos("Math");
            foreach (var a in alumnos)
            {
                promedio += a.Nota;
            }

            if((promedio / alumnos.Count )< 60)
                _mailManager.SendFlunkNotification();

            return promedio/alumnos.Count;
        }

    }

    public interface IAlumnoRepository
    {
        List<Alumno> GetAlumnos(string clase);
    }
    
    public interface IMailManager
    {
        void SendFlunkNotification();
    }

    public class Alumno
    {
        public string Nombre;
        public double Nota;
    }
}
