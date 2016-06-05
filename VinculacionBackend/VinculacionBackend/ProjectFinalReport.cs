using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using VinculacionBackend.Data.Entities;
using VinculacionBackend.Data.Interfaces;
using VinculacionBackend.Interfaces;
using Section = Spire.Doc.Section;

namespace VinculacionBackend
{
    public class ProjectFinalReport : IDownloadbleFile,ITextDoucment
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectFinalReport(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public HttpResponseMessage ToHttpResponseMessage(Document document)
        {
            var ms = new MemoryStream();
            document.SaveToStream(ms,FileFormat.Docx);
            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(ms) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "FinalReport.docx"
            };
            return response;
        }

        public HttpResponseMessage GetReport(Project project)
        {
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
            ParagraphStyle p1Style = new ParagraphStyle(doc)
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
            AddTextToParagraph("Información General", p1, p1Style, doc);
            var table1 = CreateTable(page1);
            var section = _projectRepository.GetSection(project);
            string[][] table1Data =
            {
                new[] {"Nombre del producto entregado", project.Name},
                new[] {"Nombre de la organización beneficiada", project.BeneficiariesAlias},
                new[] {"Nombre de la asignatura",section.Class.Name},
                new[] {"Nombre de la carrera", "ewfweofnweofnwe"},
                new[] {"Nombre del catedrático", "ewfweofnweofnwe"},
                new[] {"Periodo del Proyecto", "ewfweofnweofnwe"}

            };
            AddDataToTable(table1, table1Data, 2, "Times New Roman", 12);
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
                    dataRow.Cells[c].CellFormat.VerticalAlignment = VerticalAlignment.Top;
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