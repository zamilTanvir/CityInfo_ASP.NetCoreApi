namespace CityInfo.API.Models.Services
{
    public class LocalMailService : ILocalMailService
    {
        private string mailTo = "zamiltanvir2@gmail.com";
        private string From = "zamiltanvir2@gmail.com";

        public void Send(string subject, string body)
        {
            Console.WriteLine($"Mail to {mailTo} from {From} " +
                $"with {nameof(LocalMailService)}");
            Console.WriteLine("Subject: " + $"{subject}");
            Console.WriteLine("Body: " + $"{body}");
        }
    }
}
