using LinkDev.Talabat.Core.Application.Abstraction.Models._Common;

namespace LinkDev.Talabat.Core.Application.Abstraction.Services.Emails
{
    public interface IEmailSettings
    {
        public Task SendEmail(Email email);
    }
}
