using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Spire.Doc;
using Spire.Doc.Documents;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class FiniquitoReport : IDownloadbleFile
    {
        private readonly ITextDocumentServices _textDoucmentServices;

        public FiniquitoReport(ITextDocumentServices textDocumentServices)
        {

            _textDoucmentServices = textDocumentServices;
        }

        public HttpResponseMessage GetReport()
        {
            var doc = _textDoucmentServices.CreaDocument();
            var page1 = _textDoucmentServices.CreatePage(doc);
            var header = _textDoucmentServices.CreateHeader(doc);
            var headerParagraph = _textDoucmentServices.CreateHeaderParagraph(header);
            var picture1 = _textDoucmentServices.AppendPictureToHeader(headerParagraph,Properties.Resources.LaurateLogo,39f,122f,15f,-2f);
            var picture2 = _textDoucmentServices.AppendPictureToHeader(headerParagraph,Properties.Resources.UnitecFullLogo, 51f, 151f, 430f, -12f);
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
                FileName = "Finiquito.docx"
            };
            return response;
        }
    }
}