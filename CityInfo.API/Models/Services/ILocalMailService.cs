namespace CityInfo.API.Models.Services
{
    public interface ILocalMailService
    {
        void Send(string subject, string body);
    }
}