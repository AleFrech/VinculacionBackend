using RestSharp;

namespace VinculacionBackend.Interfaces
{
    public interface IEmail
    {
        IRestResponse Send(string emailAdress, string message, string subject);
    }
}
