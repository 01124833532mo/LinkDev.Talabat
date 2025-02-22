using LinkDev.Talabat.Core.Application.Abstraction.Models._Common;
using LinkDev.Talabat.Core.Application.Abstraction.Services.Emails;
using LinkDev.Talabat.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LinkDev.Talabat.Core.Application.Services.Emails
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSettings _emailSettings;

        public EmailNotificationService(UserManager<ApplicationUser> userManager, IEmailSettings emailSettings)
        {
            _userManager = userManager;
            _emailSettings = emailSettings;
        }

        public async Task SendMonthlyEmails()
        {
            var emails = await _userManager.Users.Select(e => e.Email).ToListAsync();


            foreach (var email in emails)
            {
                var emailDto = new Email
                {
                    To = email ?? string.Empty,
                    Subject = "Monthly Newsletter",
                    Body = "Wellow His is The first!"
                };

                await _emailSettings.SendEmail(emailDto);
            }
        }
    }
}
