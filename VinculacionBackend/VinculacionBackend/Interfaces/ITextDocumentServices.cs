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

        TextRange AddTextToParagraph(string text, Paragraph paragraph, ParagraphStyle style, Document document,
            HorizontalAlignment aligment = HorizontalAlignment.Left, float linespacing = 0);

        void AppendTextToFooter(Paragraph footerParagraph, string text, string fontName, float fontSize);

        void SetPageMArgins(Section page, float top, float bottom, float left, float right);

        ParagraphStyle CreateParagraphStyle(Document doc, string styleName, string fontName, float fontSize, bool bold);
        DocPicture CreateImage(Paragraph p, Bitmap image);
        Paragraph CreateParagraph(Section page);
        Section CreatePage(Document doc);
        Document CreaDocument();
        Table CreateTable(Section page);
        HeaderFooter CreateHeader(Document doc);
        HeaderFooter CreaFooter(Document doc);
        Paragraph CreateHeaderParagraph(HeaderFooter header);
        Paragraph CreateFooterParagraph(HeaderFooter footer);
    }
}