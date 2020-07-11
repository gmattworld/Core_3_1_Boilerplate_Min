using APP.Services.Email.Extension;
using System.Threading.Tasks;

namespace APP.Services.Email
{
	public interface IEmailService
	{
		void SendEmail(Message message);
		Task SendEmailAsync(Message message);
	}
}
