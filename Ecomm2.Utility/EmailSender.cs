using Microsoft.AspNetCore.Identity.UI.Services;

namespace Ecomm2.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
        
    }
}
