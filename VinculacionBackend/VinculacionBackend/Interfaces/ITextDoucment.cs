using System.Drawing;
using System.IO;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;


namespace VinculacionBackend.Interfaces
{
    public interface ITextDoucment
    {
        Document CreateDocument();
        Section CreatePage(Document document);
        DocPicture AddImage(Paragraph paragraph, Bitmap resourceImage);
        Paragraph CreateParagraph(Section page);
        void AddTextToParagraph(string text, Paragraph paragraph, ParagraphStyle style, Document document);
        void AddImageToParagraph(Paragraph paragraph, Bitmap resourceImage, float height, float width,
            TextWrappingStyle textWrappingStyle);


    }
}
