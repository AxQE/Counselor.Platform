namespace Counselor.Platform.Data.Options
{
	public class DatabaseOptions
	{
		public const string SectionName = "Platform:Database";
		public string Server { get; set; }
		public int Port { get; set; }
		public string UserId { get; set; }
		public string Password { get; set; }
		public string Database { get; set; }

		public string BuildConnectionString()
		{
			return $"Server={Server};Port={Port};User Id={UserId};Password={Password};Database={Database};";
		}
	}
}
