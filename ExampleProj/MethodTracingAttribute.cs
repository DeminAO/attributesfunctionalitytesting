using System;

namespace ExampleProj.MethodCallTracing
{
	[AttributeUsage(AttributeTargets.Method)]
	public class MethodTracingAttribute : Attribute
	{
		public string Before { get; }
		public string After { get; }

		///
		public MethodTracingAttribute(string BeforeActionName, string AfterActionName)
		{
			Before = BeforeActionName;
			After = AfterActionName;
		}

		///
		public MethodTracingAttribute(string ActionName, bool IsBefor = true)
		{
			if (IsBefor)
				Before = ActionName;
			else
				After = ActionName;
		}
	}
}