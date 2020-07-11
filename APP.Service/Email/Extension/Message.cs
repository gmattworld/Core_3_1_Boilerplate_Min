using MimeKit;
using System.Collections.Generic;
using System.Linq;

namespace APP.Services.Email.Extension
{
	public class Message
	{
		public List<MailboxAddress> To { get; set; }
		public string Subject { get; set; }
		public string Body { get; set; }

		public Message(IEnumerable<string> to, string subject, string body)
		{
			To = new List<MailboxAddress>();

			To.AddRange(to.Select(x => new MailboxAddress(x)));
			Subject = subject;
			Body = body;
		}
	}
}
