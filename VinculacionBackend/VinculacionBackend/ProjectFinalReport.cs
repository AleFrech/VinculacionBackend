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
            ParagraphStyle p0Style = CreateParagraphStyle(doc, "HeaderStyle", "Times New Roman", 14f, true);
            AddTextToParagraph(titleHeader, p0, p0Style, doc);
            AddImageToParagraph(p0, Properties.Resources.UnitecLogo,59F,69F,TextWrappingStyle.Square);
            var p1 = CreateParagraph(page1);
            ParagraphStyle tableHeadersStyle =CreateParagraphStyle(doc, "GeneralInfo", "Times New Roman",12f,true);
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
            table1.ResetCells(table1Data.Length,2);
            AddDataToTable(table1, table1Data, "Times New Roman", 12,0);

            var p2 = CreateParagraph(page1);
            AddTextToParagraph("\r\nCaracterísticas del Proyecto", p2, tableHeadersStyle, doc);

            var table2 = CreateTable(page1);
            string[][] table2Data =
            {
                new[] { "Grupo(s) meta beneficiado(s) con el producto entregado",project.BeneficiarieGroups},
                new[] { "Número de personas beneficiadas", project.BeneficiariesQuantity.ToString()}
            };
            table2.ResetCells(table2Data.Length, 2);
            AddDataToTable(table2, table2Data, "Times New Roman", 12,0);
    
            var p3 = CreateParagraph(page1);
            AddTextToParagraph("\r\nTiempo y valor del producto ", p3, tableHeadersStyle, doc);

            var table3 = CreateTable(page1);
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
            AddDataToTable(table3, table3Data, "Times New Roman", 12,0);
            var p4 = CreateParagraph(page1);
            ParagraphStyle p4Style = CreateParagraphStyle(doc, "3tableStyle", "Times New Roman",8,false);
            AddTextToParagraph("*Se refiere a la evaluación que hace el catedrático sobre la calidad del proyecto",p4,p4Style,doc);

            var p5 = CreateParagraph(page1);
            AddTextToParagraph("\r\n\r\nEstudiantes Involucrados en el Proyecto de Vinculación",p5,tableHeadersStyle,doc);

            var table4 = CreateTable(page1);
            string[] headerTable4 = { "No.", "Cuenta", "Nombre y Apellidos", "Horas por alumno", "Firma del estudiante" };
            AddDataToTableWithHeader(table4,headerTable4,table4Data,5, "Times New Roman", 12);
            var p6 = CreateParagraph(page1);
            ParagraphStyle p6Style = CreateParagraphStyle(doc, "lastParagraphStyle", "Times New Roman", 12f, false);
            AddTextToParagraph("\r\n\r\n\r\n\r\nFirma del docente__________________________	Fecha de entrega___________________",p6,p6Style,doc);
            return ToHttpResponseMessage(doc);
        }

        private void AddDataToTable(Table table ,string[][] data,string font, float fontsize,int offset)
        {

            for (int r = 0; r < data.Length; r++)
            {
                TableRow dataRow = table.Rows[r+offset];
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

        private void AddDataToTableWithHeader(Table table,string[] header,string[][] data, int columnCount, string font, float fontsize)
        {
            table.ResetCells(data.Length+1, columnCount);

            TableRow Frow = table.Rows[0];
            Frow.IsHeader = true;
            Frow.Height = 30;
           
            Frow.HeightType = TableRowHeightType.Exactly;
            Frow.RowFormat.BackColor = Color.LightGray;
            for (int i = 0; i < header.Length; i++)
            {
                Frow.Cells[i].CellFormat.VerticalAlignment = VerticalAlignment.Middle;
                Paragraph p = Frow.Cells[i].AddParagraph();
                p.Format.HorizontalAlignment = HorizontalAlignment.Center;
                TextRange txtRange = p.AppendText(header[i]);
                txtRange.CharacterFormat.FontName = font;
                txtRange.CharacterFormat.FontSize = fontsize;
                if (i == header.Length - 1)
                {
                    txtRange.CharacterFormat.Bold = true;
                    txtRange.CharacterFormat.TextColor = Color.Red;
                }
            }
            AddDataToTable(table,data,font,fontsize,1);
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

        public void AddImageToParagraph(Paragraph paragraph, Bitmap resourceImage, float height, float width,TextWrappingStyle textWrappingStyle)
        {
            var picture = AddImage(paragraph, resourceImage);
            picture.Height = height;
            picture.Width = width;
            picture.TextWrappingStyle = textWrappingStyle;
        }
        
        public void AddTextToParagraph(string text,Paragraph paragraph,ParagraphStyle style,Document document)
        {
            paragraph.AppendText(text);
            document.Styles.Add(style);
            paragraph.ApplyStyle(style.Name);
        }

        public Document CreateDocument()
        {
            return new Document();
        }

        public ParagraphStyle CreateParagraphStyle(Document doc,string styleName,string fontName,float fontSize,bool bold)
        {
            return new ParagraphStyle(doc)
            {
                Name = styleName,
                CharacterFormat =
                {
                    FontName = fontName,
                    FontSize =fontSize,
                    Bold = bold,
                }
            };
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