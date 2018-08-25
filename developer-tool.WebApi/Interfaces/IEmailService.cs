using System.Threading.Tasks;
namespace Email.Services
{
    public interface IEmailService
    {
        Task SendEmail(string name, string phone, string email, string subject, string message);
    }
}