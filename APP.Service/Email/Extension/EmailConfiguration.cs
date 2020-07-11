namespace APP.Services.Email.Extension
{
	public class EmailConfiguration
	{
		public string SMTPServer { get; set; }
		public int SMTPPort { get; set; }
		public string FromAddress { get; set; }
		public string FromAddressTitle { get; set; }
		public string SMTPUserName { get; set; }
		public string SMTPPassword { get; set; }
		public bool EnableSSL { get; set; }
		public bool UseDefaultCredentials { get; set; }
	}
}