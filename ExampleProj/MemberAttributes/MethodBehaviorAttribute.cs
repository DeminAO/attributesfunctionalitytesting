using System;

namespace ExampleProj.MethodCallTracing
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class MethodBehaviorAttribute : Attribute
	{
		public string ActionName { get; protected set; }
	}
}