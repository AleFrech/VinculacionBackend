using System.Drawing;
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
        Table CreateTable(Section page1);
        HeaderFooter CreateHeader(Document doc);
    }
}
