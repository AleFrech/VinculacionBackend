using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using Section = Spire.Doc.Section;

namespace VinculacionBackend
{
    public class ProjectFinalReport : IDownloadbleFile,ITextDoucment
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ISectionRepository _sectionRepository;
        private readonly IStudentRepository _studentRepository;

        public ProjectFinalReport(IProjectRepository projectRepository, ISectionRepository sectionRepository, IStudentRepository studentRepository)
        {
            _projectRepository = projectRepository;
            _sectionRepository = sectionRepository;
            _studentRepository = studentRepository;
        }


        public HttpResponseMessage GetReport(long projectId, int fieldHours, int calification)
        {
            var project = _projectRepository.Get(projectId);
            var doc = CreateDocument();
            var page1 = CreatePage(doc);
            var p0 = CreateParagraph(page1);
            var titleHeader = "   UNIVERSIDAD TECNOLOGICA CENTROAMERICANA\r\n " +
                          "                                              UNITEC\r\n " +
                          "       Dirección de Investigación y Vinculación Universitaria\r\n" +
                          "                      Evaluación de Proyecto de Vinculación";
            ParagraphStyle p0Style = new ParagraphStyle(doc)
            {
                Name = "HeaderStyle",
                CharacterFormat =
                {
                    FontName = "Times New Roman",
                    FontSize = 14,
                    Bold = true,
                }
            };

            AddTextToParagraph(titleHeader, p0, p0Style, doc);
            AddImageToParagraph(p0, Properties.Resources.UnitecLogo,59F,69F,TextWrappingStyle.Square);
            var p1 = CreateParagraph(page1);
            ParagraphStyle tableHeadersStyle = new ParagraphStyle(doc)
            {
                Name = "GeneralInfo",
                CharacterFormat =
                {
                    FontName = "Times New Roman",
                    FontSize = 12,
                    Bold = true,
                    UnderlineStyle = UnderlineStyle.Thick
                }
            };
            AddTextToParagraph("Información General", p1, tableHeadersStyle, doc);
            var table1 = CreateTable(page1);
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
            AddDataToTable(table1, table1Data, 2, "Times New Roman", 12);

            var p2 = CreateParagraph(page1);
            AddTextToParagraph("\r\nCaracterísticas del Proyecto", p2, tableHeadersStyle, doc);

            var table2 = CreateTable(page1);
            string[][] table2Data =
            {
                new[] { "Grupo(s) meta beneficiado(s) con el producto entregado",project.BeneficiarieGroups},
                new[] { "Número de personas beneficiadas", project.BeneficiariesQuantity.ToString()}
            };
            AddDataToTable(table2, table2Data, 2, "Times New Roman", 12);

            
            var p3 = CreateParagraph(page1);
            AddTextToParagraph("\r\nTiempo y valor del producto ", p3, tableHeadersStyle, doc);

            var table3 = CreateTable(page1);
            string[][] table3Data =
            {
                new[] { "Horas de trabajo de campo alumnos ", fieldHours.ToString()},
                new[] { "Horas de trabajo en clase alumnos ", "ewt3e53453"},
                new[] { "Total Horas de Trabajo del Proyecto", "345345ee4g"},
                new[] { "Nota asignada al proyecto (%)*",calification+"%"},
                new[] { "Valor en el mercado del producto (Lps.)", project.Cost.ToString()},
            };
            AddDataToTable(table3, table3Data, 2, "Times New Roman", 12);
            var p4 = CreateParagraph(page1);
            ParagraphStyle p4Style = new ParagraphStyle(doc)
            {
                Name = "3tableStyle",
                CharacterFormat =
                {
                    FontName = "Times New Roman",
                    FontSize = 8,
                }
            };
            AddTextToParagraph("*Se refiere a la evaluación que hace el catedrático sobre la calidad del proyecto",p4,p4Style,doc);

            var p5 = CreateParagraph(page1);
            AddTextToParagraph("\r\n\r\nEstudiantes Involucrados en el Proyecto de Vinculación",p5,tableHeadersStyle,doc);


            return ToHttpResponseMessage(doc);
           }

        private void AddDataToTable(Table table ,string[][] data,int columnCount,string font, float fontsize)
        {
            table.ResetCells(data.Length, columnCount);

            for (int r = 0; r < data.Length; r++)
            {
                var dataRow = table.Rows[r];
                dataRow.Height = 20;
                for (int c = 0; c < data[r].Length; c++)
                {
                    dataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                    Paragraph p2 = dataRow.Cells[c].AddParagraph();
                    TextRange tr2 = p2.AppendText(data[r][c]);
                    p2.Format.HorizontalAlignment = HorizontalAlignment.Left;
                    tr2.CharacterFormat.FontName = font;
                    tr2.CharacterFormat.FontSize = fontsize;
                    tr2.CharacterFormat.Bold = true;
                }

            }
            table.TableFormat.Borders.BorderType = BorderStyle.Single;
            table.TableFormat.HorizontalAlignment = RowAlignment.Left;
            table.TableFormat.LeftIndent = 8f;
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

        private Table CreateTable(Section page1)
        {
            return page1.AddTable();
        }

        private void AddImageToParagraph(Paragraph paragraph, Bitmap resourceImage, float height, float width,TextWrappingStyle textWrappingStyle)
        {
            var picture = AddImage(paragraph, resourceImage);
            picture.Height = height;
            picture.Width = width;
            picture.TextWrappingStyle = textWrappingStyle;
        }
        
        private void AddTextToParagraph(string text,Paragraph paragraph,ParagraphStyle style,Document document)
        {
            paragraph.AppendText(text);
            document.Styles.Add(style);
            paragraph.ApplyStyle(style.Name);
        }

        public Document CreateDocument()
        {
            return new Document();
        }

        public Section CreatePage(Document document)
        {

            return document.AddSection();
        }

        public DocPicture AddImage(Paragraph paragraph, Bitmap resourceImage)
        {
            return paragraph.AppendPicture(resourceImage);
        }

        public Paragraph CreateParagraph(Section page)
        {
            return page.AddParagraph();
        }

        

    }
}