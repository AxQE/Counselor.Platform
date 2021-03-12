using System;

namespace Counselor.Platform.Options
{
	class PlatformOptions
	{
		public const string SectionName = "Platform";
		public string DialogsPath { get; set; } = $"{AppDomain.CurrentDomain.BaseDirectory}Dialogs";
		public DatabaseOptions Database { get; set; }
	}
}
