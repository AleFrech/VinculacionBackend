using System.Drawing;
using Spire.Doc;
using Spire.Doc.Documents;
using Spire.Doc.Fields;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class TextDocument:ITextDoucment
    {
        public Table CreateTable(Section page1)
        {
            return page1.AddTable();
        }

        public HeaderFooter CreateHeader(Document doc)
        {
            return doc.Sections[0].HeadersFooters.Header;
        }

        public HeaderFooter CreaFooter(Document doc)
        {
            return doc.Sections[0].HeadersFooters.Footer;
        }
 

        public Document CreateDocument()
        {
            return new Document();
        }

        public ParagraphStyle CreateParagraphStyle(Document doc, string styleName, string fontName, float fontSize, bool bold)
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

            var page= document.AddSection();
            page.PageSetup.PageSize = PageSize.Letter;
            return page;

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