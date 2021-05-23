using Counselor.Platform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Counselor.Platform.Interpreter
{
	public class InterpreterRuntime : IInterpreter
	{
		public async Task<InterpretationResult> Interpret(IInstruction instruction, Dialog dialog)
		{			
			return default;
		}

		public void Dispose()
		{
			//todo: dispose
		}
	}
}
