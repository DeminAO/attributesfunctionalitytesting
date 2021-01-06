using System;

namespace ExampleProj.MethodCallTracing
{
	[AttributeUsage(AttributeTargets.Method)]
	public class MethodTracingAttribute : Attribute
	{
		public string Before { get; }
		public string After { get; }

		///
		public MethodTracingAttribute(string beforeActionName, string afterActionName)
		{
			Console.WriteLine($"{nameof(MethodTracingAttribute)} -> ctor");
			Before = beforeActionName;
			After = afterActionName;
		}
	}
}