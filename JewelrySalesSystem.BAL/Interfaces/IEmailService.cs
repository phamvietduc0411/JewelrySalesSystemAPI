using JewelrySalesSystem.BAL.Helpers;

namespace JewelrySalesSystem.BAL.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
