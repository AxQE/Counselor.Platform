using System;

namespace Counselor.Platform.Interpreter.Commands
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class InterpreterCommandAttribute : Attribute
	{
		public string ParameterName { get; private set; }
		public Type ParameterType { get; private set; }		
		public bool IsActive { get; private set; }

		public InterpreterCommandAttribute(string parameterName, Type parameterType, bool isActive = true)
		{
			ParameterName = string.IsNullOrEmpty(parameterName) ? "Parameter" : parameterName;
			ParameterType = parameterType;
			IsActive = isActive;
		}
	}
}
