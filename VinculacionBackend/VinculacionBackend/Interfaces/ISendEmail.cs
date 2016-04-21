using RestSharp;

namespace VinculacionBackend.Interfaces
{
    public interface ISendEmail
    {
        IRestResponse Send(string emailAdress, string message, string subject);
    }
}
