using System;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace VinculacionBackend.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var alumnosRepositoryMock = new Mock<IAlumnoRepository>();
            var mailManagerMock = new Mock<IMailManager>();
            var alumnos = new List<Alumno>
            {
                new Alumno {Nombre = "A", Nota = 70},
                new Alumno {Nombre = "B", Nota = 80},
                new Alumno {Nombre = "C", Nota = 60}
            };
            alumnosRepositoryMock.Setup(rep => rep.GetAlumnos(It.IsAny<string>())).Returns(alumnos);
            var reporte = new Reporte(alumnosRepositoryMock.Object,mailManagerMock.Object);
            var promedio=reporte.ObtenerPromedio();
            mailManagerMock.Verify(rep=>rep.SendFlunkNotification(),Times.Never);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var alumnosRepositoryMock = new Mock<IAlumnoRepository>();
            var mailManagerMock = new Mock<IMailManager>();

            var alumnos = new List<Alumno>
            {
                new Alumno {Nombre = "A", Nota = 30},
                new Alumno {Nombre = "B", Nota = 50},
                new Alumno {Nombre = "C", Nota = 40}
            };
            alumnosRepositoryMock.Setup(rep => rep.GetAlumnos(It.IsAny<string>())).Returns(alumnos);
            var reporte = new Reporte(alumnosRepositoryMock.Object,mailManagerMock.Object);
            var promedio = reporte.ObtenerPromedio();
            mailManagerMock.Verify(rep=>rep.SendFlunkNotification());
        }

    }
}
