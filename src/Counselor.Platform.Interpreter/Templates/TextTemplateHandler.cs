using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Templates
{
	class TextTemplateHandler
	{
		private static readonly Regex TemplateRegex = new Regex("{(.+?)}", RegexOptions.Compiled);		

		public TextTemplateHandler()
		{

		}

		public async Task<string> InsertEntityParameters(string textTemplate, Dialog dialog, IPlatformDatabase database)
		{
			//todo: нужно считать хеш шаблона, хранить его возможно прямо в объекте шага поведения, после кешировать хеш и параметры шаблона
			var parameters = TemplateRegex.Matches(textTemplate)
				.OfType<Match>()
				.Select(x => x.Groups[0].Value)
				.ToDictionary(parameter => parameter.Trim('{').Trim('}'), parameter => parameter);

			if (!parameters.Any()) return textTemplate;

			StringBuilder resultString = new StringBuilder(textTemplate);

			foreach (var p in parameters.Keys)
			{
				var entityParameter = p.Split(':');

				if (!EntityNameIsValid(entityParameter[0]))
					throw new KeyNotFoundException($"Text template processing error. Entity name not found. {p[0]}");

				if (entityParameter[0].Equals("user", StringComparison.OrdinalIgnoreCase))
				{
					var value = dialog.User.GetType()
						.GetProperties()
						.FirstOrDefault(x => x.Name.Equals(entityParameter[1], StringComparison.OrdinalIgnoreCase))
						.GetValue(dialog.User).ToString();

					resultString.Replace(parameters[p], value);					
				}
				else
				{

				}
			}

			return resultString.ToString();
		}

		private bool EntityNameIsValid(string entityName)
		{
			return true;
		}
	}
}
