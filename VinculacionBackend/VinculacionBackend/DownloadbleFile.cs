using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Spire.Doc;
using VinculacionBackend.Interfaces;

namespace VinculacionBackend
{
    public class DownloadbleFile:IDownloadbleFile
    {
        public HttpResponseMessage ToHttpResponseMessage(Document document, string fileName)
        {
            var ms = new MemoryStream();
            document.SaveToStream(ms, FileFormat.Docx);
            ms.Position = 0;
            var response = new HttpResponseMessage(HttpStatusCode.OK) { Content = new StreamContent(ms) };
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };
            return response;
        }
    }
}