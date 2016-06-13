using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Spire.Doc;
using Spire.Doc.Documents;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend.Reports
{
    public class ProjectFinalReport : IDownloadbleFile
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITextDocumentServices _textDoucmentServices;

        public ProjectFinalReport(IProjectRepository projectRepository, ISectionRepository sectionRepository, IStudentRepository studentRepository, ITextDocumentServices textDocumentServices)
        {
            _projectRepository = projectRepository;
            _sectionRepository = sectionRepository;
            _studentRepository = studentRepository;
            _textDoucmentServices = textDocumentServices;
        }


        public HttpResponseMessage GenerateFinalReport(long projectId, int fieldHours, int calification)
        {
            var project = _projectRepository.Get(projectId);
            var doc = _textDoucmentServices.CreaDocument();
            var page1 = _textDoucmentServices.CreatePage(doc);
            var pblank = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle p0Style = _textDoucmentServices.CreateParagraphStyle(doc, "HeaderStyle", "Times New Roman", 14f, true);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n",pblank,p0Style,doc);        
            var titleHeader = "   UNIVERSIDAD TECNOLOGICA CENTROAMERICANA\r\n " +
                          "                                              UNITEC\r\n " +
                          "       Dirección de Investigación y Vinculación Universitaria\r\n" +
                          "                      Evaluación de Proyecto de Vinculación";
            var p0 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph(titleHeader, p0, p0Style, doc);
            _textDoucmentServices.AddImageToParagraph(p0, Properties.Resources.UnitecLogo,59F,69F,TextWrappingStyle.Square);
            var p1 = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle tableHeadersStyle = _textDoucmentServices.CreateParagraphStyle(doc, "GeneralInfo", "Times New Roman",12f,true);
            _textDoucmentServices.AddTextToParagraph("Información General", p1, tableHeadersStyle, doc);
            var table1 = _textDoucmentServices.CreateTable(page1);
            var section = _projectRepository.GetSection(project);
            var studentsInSection = _sectionRepository.GetSectionStudents(section.Id).ToList();
            var majorsOfStudents = _studentRepository.GetStudentMajors(studentsInSection);
            string[][] table1Data =
            {
                new[] {"Nombre del producto entregado", project.Name},
                new[] {"Nombre de la organización beneficiada", project.BeneficiarieOrganization},
                new[] {"Nombre de la asignatura",section.Class.Name},
                new[] {"Nombre de la carrera",majorsOfStudents},
                new[] {"Nombre del catedrático", section.User.Name},
                new[] {"Periodo del Proyecto", "Desde   "+section.Period.FromDate+"   Hasta   "+section.Period.ToDate}

            };
            table1.ResetCells(table1Data.Length,2);
            _textDoucmentServices.AddDataToTable(table1, table1Data, "Times New Roman", 12,0);

            var p2 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\nCaracterísticas del Proyecto", p2, tableHeadersStyle, doc);

            var table2 = _textDoucmentServices.CreateTable(page1);
            string[][] table2Data =
            {
                new[] { "Grupo(s) meta beneficiado(s) con el producto entregado",project.BeneficiarieGroups},
                new[] { "Número de personas beneficiadas", project.BeneficiariesQuantity.ToString()}
            };
            table2.ResetCells(table2Data.Length, 2);
            _textDoucmentServices.AddDataToTable(table2, table2Data, "Times New Roman", 12,0);
    
            var p3 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\nTiempo y valor del producto ", p3, tableHeadersStyle, doc);

            var table3 = _textDoucmentServices.CreateTable(page1);
            var studentsHours = _studentRepository.GetStudentsHoursByProject(projectId);
            var totalHours = 0;
            string[][] table4Data = new string[studentsHours.Count][];
            var i = 0;
            foreach (var sh in studentsHours)
            {
                table4Data[i] = new[] { (i + 1).ToString(), sh.Key.AccountId, sh.Key.Name, sh.Value.ToString(), "" };
                i++;
                totalHours += sh.Value;
            }
            string[][] table3Data =
            {
                new[] { "Horas de trabajo de campo alumnos ", fieldHours.ToString()},
                new[] { "Horas de trabajo en clase alumnos ", (totalHours-fieldHours).ToString()},
                new[] { "Total Horas de Trabajo del Proyecto", totalHours.ToString()},
                new[] { "Nota asignada al proyecto (%)*",calification+"%"},
                new[] { "Valor en el mercado del producto (Lps.)", project.Cost.ToString()},
            };
            table3.ResetCells(table3Data.Length, 2);
            _textDoucmentServices.AddDataToTable(table3, table3Data, "Times New Roman", 12,0);
            var p4 = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle p4Style = _textDoucmentServices.CreateParagraphStyle(doc, "3tableStyle", "Times New Roman",8,false);
            _textDoucmentServices.AddTextToParagraph("*Se refiere a la evaluación que hace el catedrático sobre la calidad del proyecto",p4,p4Style,doc);

            var p5 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\nEstudiantes Involucrados en el Proyecto de Vinculación",p5,tableHeadersStyle,doc);

            var table4 = _textDoucmentServices.CreateTable(page1);
            string[] headerTable4 = { "No.", "Cuenta", "Nombre y Apellidos", "Horas por alumno", "Firma del estudiante" };
            _textDoucmentServices.AddDataToTableWithHeader(table4,headerTable4,table4Data,5, "Times New Roman", 12);
            var p6 = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle p6Style = _textDoucmentServices.CreateParagraphStyle(doc, "lastParagraphStyle", "Times New Roman", 12f, false);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n\r\n\r\nFirma del docente__________________________	Fecha de entrega___________________",p6,p6Style,doc);
            return ToHttpResponseMessage(doc);
        }

        public HttpResponseMessage ToHttpResponseMessage(Document document)
        {
            var ms = new MemoryStream();
            document.SaveToStream(ms, FileFormat.Docx);
            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(ms) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "FinalReport.docx"
            };
            return response;
        }
    }
}