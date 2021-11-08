using DisneyAPI.ViewModels.Services.MailService;
using System.Threading.Tasks;

namespace DisneyAPI.Interfaces
{
    public interface IMailService
    {
        Task SendEmail(MailServiceRequestViewModel model);
    }
}