using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter.Templates
{
	interface ITextTemplateHandler
	{
		Task<string> InsertEntityParameters(string messageTemplate);
	}
}
