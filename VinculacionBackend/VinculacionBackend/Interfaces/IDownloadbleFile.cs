using System.Net.Http;
using Spire.Doc;

namespace VinculacionBackend.Interfaces
{
    public interface IDownloadbleFile
    {
        HttpResponseMessage ToHttpResponseMessage(Document document,string fileName);
    }
}
