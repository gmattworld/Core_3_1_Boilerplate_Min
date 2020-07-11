using APP.Services.Email;
using APP.Services.Email.Extension;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APP.API.Utilities
{
    public static class Helpers
    {
        public static async Task Send(this IEmailService emailService, IEnumerable<string> EmailAddress, string subject, string body, bool sendAsync = true)
        {
            try
            {
                if (sendAsync)
                {
                    var message = new Message(EmailAddress, subject, body);
                    await emailService.SendEmailAsync(message);
                }
                else
                {
                    var message = new Message(EmailAddress, subject, body);
                    emailService.SendEmail(message);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public static async Task Send(this IEmailService emailService, string EmailAddress, string subject, string body, bool sendAsync = true)
        {
            IEnumerable<string> _emailAddress = new string[]
            {
                EmailAddress
            };

            _ = Send(emailService, _emailAddress, subject, body, sendAsync).ConfigureAwait(false);
        }
    }
}
