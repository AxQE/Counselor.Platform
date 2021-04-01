using System;

namespace Counselor.Platform.Data.Options
{
	public class PlatformOptions
	{
		public const string SectionName = "Platform";
		public string DialogsPath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}Dialogs";
		public DatabaseOptions Database { get; set; }
	}
}
