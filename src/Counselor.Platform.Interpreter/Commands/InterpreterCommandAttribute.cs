using System;

namespace Counselor.Platform.Interpreter.Commands
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class InterpreterCommandAttribute : Attribute
	{
		public string ParameterName { get; }
		public Type ParameterType { get; }
		public bool IsActive { get; }

		public InterpreterCommandAttribute(string parameterName, Type parameterType, bool isActive = true)
		{
			ParameterName = string.IsNullOrEmpty(parameterName) ? "Parameter" : parameterName;
			ParameterType = parameterType;
			IsActive = isActive;
		}
	}
}
