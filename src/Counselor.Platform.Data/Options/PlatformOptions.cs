﻿using System;

namespace Counselor.Platform.Data.Options
{
	public class PlatformOptions
	{
		public const string SectionName = "Platform";
		public static string DialogsPath { get => $"{AppDomain.CurrentDomain.BaseDirectory}Dialogs"; }
		public DatabaseOptions Database { get; set; }
	}
}
