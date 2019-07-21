using System.Threading.Tasks;
namespace Core.Interfaces
{
    public interface IEmailService
    {
        Task SendEmail(string name, string phone, string email, string subject, string message);
    }
}