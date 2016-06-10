using System.Drawing;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;

namespace VinculacionBackend.Interfaces
{
    public interface ITextDocumentServices
    {
        DocPicture AppendPictureToHeader(Paragraph headParagraph, Bitmap image, float height, float width,
            float horizontalPosition, float verticalPosition);
        void AddDataToTable(Table table, string[][] data, string font, float fontsize, int offset);
        void AddDataToTableWithHeader(Table table, string[] header, string[][] data, int columnCount,
            string font, float fontsize);
        void AddImageToParagraph(Paragraph paragraph, Bitmap image, float height, float width,
            TextWrappingStyle textWrappingStyle);
        void AddTextToParagraph(string text, Paragraph paragraph, ParagraphStyle style, Document document);
        ParagraphStyle CreateParagraphStyle(Document doc, string styleName, string fontName, float fontSize, bool bold);
        DocPicture CreateImage(Paragraph p, Bitmap image);
        Paragraph CreateParagraph(Section page);
        Section CreatePage(Document doc);
        Document CreaDocument();
        Table CreateTable(Section page);
        HeaderFooter CreateHeader(Document doc);
        Paragraph CreateHeaderParagraph(HeaderFooter header);
    }
}