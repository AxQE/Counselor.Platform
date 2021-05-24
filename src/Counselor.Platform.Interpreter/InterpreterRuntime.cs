﻿using Counselor.Platform.Data.Database;
using Counselor.Platform.Data.Entities;
using Counselor.Platform.Interpreter.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	public class InterpreterRuntime : IInterpreter
	{
		private static readonly ITextTemplateHandler _templateHandler = new TextTemplateHandler();

		public async Task<InterpretationResult> Interpret(IInstruction instruction, Dialog dialog, IPlatformDatabase database)
		{			
			return new InterpretationResult();
		}

		public async Task<string> FillEntityParameters(string message, Dialog dialog, IPlatformDatabase database)
		{
			throw new NotImplementedException();
		}

		public void Dispose()
		{
			//todo: dispose
		}		
	}
}
