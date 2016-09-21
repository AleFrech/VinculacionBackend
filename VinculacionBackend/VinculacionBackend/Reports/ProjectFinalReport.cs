using System.Globalization;
using System.Linq;
using System.Net.Http;
using Spire.Doc.Documents;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using VinculacionBackend.Models;

namespace VinculacionBackend.Reports
{
    public class ProjectFinalReport
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITextDocumentServices _textDoucmentServices;
        private readonly IDownloadbleFile _downloadbleFile;
        private readonly ISectionProjectRepository _sectionProjectRepository;

        public ProjectFinalReport(IProjectRepository projectRepository, ISectionRepository sectionRepository, IStudentRepository studentRepository, ITextDocumentServices textDocumentServices, IDownloadbleFile downloadbleFile, ISectionProjectRepository sectionProjectRepository)
        {
            _projectRepository = projectRepository;
            _sectionRepository = sectionRepository;
            _studentRepository = studentRepository;
            _textDoucmentServices = textDocumentServices;
            _downloadbleFile = downloadbleFile;
            _sectionProjectRepository = sectionProjectRepository;
        }



        public ProjectFinalReportModel GenerateFinalReportModel(long projectId, long sectionId, long sectionprojectId, int fieldHours, int calification, int beneficiariesQuantity, string beneficiarieGroups)
        {
            var model = new ProjectFinalReportModel();
            model.Project= _projectRepository.Get(projectId);
            model.Section = _sectionRepository.Get(sectionId);
            model.BeneficiarieGroups = beneficiarieGroups;
            model.BeneficiariesQuantity = beneficiariesQuantity;
            model.Calification = calification;
            model.FieldHours = fieldHours;
            model.StudentsInSections= _sectionRepository.GetSectionStudents(sectionId).ToList();
            model.MajorsOfStudents= _studentRepository.GetStudentMajors(model.StudentsInSections);
            model.SectionProject = _sectionProjectRepository.Get(sectionprojectId);
            model.StudentsHours = _studentRepository.GetStudentsHoursByProject(sectionId,projectId);
            model.ProfessorName = model.Section.User != null ? model.Section.User.Name : "Maestro Pendiente";
            return model;
        }


        public HttpResponseMessage GenerateFinalReport(ProjectFinalReportModel model)
        {
            var doc = _textDoucmentServices.CreaDocument();
            var page1 = _textDoucmentServices.CreatePage(doc);
            var pblank = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle p0Style = _textDoucmentServices.CreateParagraphStyle(doc, "HeaderStyle", "Times New Roman",
                14f, true);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\n", pblank, p0Style, doc);
            var titleHeader = "   UNIVERSIDAD TECNOLOGICA CENTROAMERICANA\r\n " +
                              "                                              UNITEC\r\n " +
                              "       Dirección de Investigación y Vinculación Universitaria\r\n" +
                              "                      Evaluación de Proyecto de Vinculación";
            var p0 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph(titleHeader, p0, p0Style, doc);
            _textDoucmentServices.AddImageToParagraph(p0, Properties.Resources.UnitecLogo, 59F, 69F,
                TextWrappingStyle.Square);
            var p1 = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle tableHeadersStyle = _textDoucmentServices.CreateParagraphStyle(doc, "GeneralInfo",
                "Times New Roman", 12f, true);
            _textDoucmentServices.AddTextToParagraph("Información General", p1, tableHeadersStyle, doc);
            var table1 = _textDoucmentServices.CreateTable(page1);
            string[][] table1Data =
            {
                new[] {"Codigo", model.SectionProject.Id.ToString()},
                new[] {"Nombre del producto entregado", model.Project.Name},
                new[] {"Nombre de la organización beneficiada", model.Project.BeneficiarieOrganization},
                new[] {"Nombre de la asignatura", model.Section.Class.Name},
                new[] {"Nombre de la carrera", model.MajorsOfStudents},
                new[] {"Nombre del catedrático",model.ProfessorName},
                new[]
                {"Periodo del Proyecto", "Desde   " + model.Section.Period.FromDate + "   Hasta   " + model.Section.Period.ToDate}

            };
            table1.ResetCells(table1Data.Length, 2);
            _textDoucmentServices.AddDataToTable(table1, table1Data, "Times New Roman", 12, 0);

            var p2 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\nCaracterísticas del Proyecto", p2, tableHeadersStyle, doc);

            var table2 = _textDoucmentServices.CreateTable(page1);
            string[][] table2Data =
            {
                new[] {"Grupo(s) meta beneficiado(s) con el producto entregado",model.BeneficiarieGroups},
                new[] {"Número de personas beneficiadas", model.BeneficiarieGroups.ToString()}
            };
            table2.ResetCells(table2Data.Length, 2);
            _textDoucmentServices.AddDataToTable(table2, table2Data, "Times New Roman", 12, 0);

            var p3 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\nTiempo y valor del producto ", p3, tableHeadersStyle, doc);

            var table3 = _textDoucmentServices.CreateTable(page1);
            var studentsHours = model.StudentsHours;
            var totalHours = 0;
            string[][] table4Data = new string[studentsHours.Count][];
            var i = 0;
            foreach (var sh in studentsHours)
            {
                table4Data[i] = new[] {(i + 1).ToString(), sh.Key.AccountId, sh.Key.Name, sh.Value.ToString(), ""};
                i++;
                totalHours += sh.Value;
            }

            string[][] table3Data =
            {
                new[] {"Horas de trabajo de campo alumnos ", model.FieldHours.ToString()},
                new[] {"Horas de trabajo en clase alumnos ", (totalHours - model.FieldHours).ToString()},
                new[] {"Total Horas de Trabajo del Proyecto", totalHours.ToString()},
                new[] {"Nota asignada al proyecto (%)*", model.Calification + "%"},
                new[] {"Valor en el mercado del producto (Lps.)", model.SectionProject.Cost.ToString(CultureInfo.InvariantCulture)},
            };
            table3.ResetCells(table3Data.Length, 2);
            _textDoucmentServices.AddDataToTable(table3, table3Data, "Times New Roman", 12, 0);
            var p4 = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle p4Style = _textDoucmentServices.CreateParagraphStyle(doc, "3tableStyle", "Times New Roman", 8,
                false);
            _textDoucmentServices.AddTextToParagraph(
                "*Se refiere a la evaluación que hace el catedrático sobre la calidad del proyecto", p4, p4Style, doc);

            var p5 = _textDoucmentServices.CreateParagraph(page1);
            _textDoucmentServices.AddTextToParagraph("\r\n\r\nEstudiantes Involucrados en el Proyecto de Vinculación",
                p5, tableHeadersStyle, doc);

            var table4 = _textDoucmentServices.CreateTable(page1);
            string[] headerTable4 = {"No.", "Cuenta", "Nombre y Apellidos", "Horas por alumno", "Firma del estudiante"};
            _textDoucmentServices.AddDataToTableWithHeader(table4, headerTable4, table4Data, 5, "Times New Roman", 12);
            var p6 = _textDoucmentServices.CreateParagraph(page1);
            ParagraphStyle p6Style = _textDoucmentServices.CreateParagraphStyle(doc, "lastParagraphStyle",
                "Times New Roman", 12f, false);
            _textDoucmentServices.AddTextToParagraph(
                "\r\n\r\n\r\n\r\nFirma del docente__________________________	Fecha de entrega___________________", p6,
                p6Style, doc);
            return _downloadbleFile.ToHttpResponseMessage(doc, "FinalReport.docx");
        }
    }
}