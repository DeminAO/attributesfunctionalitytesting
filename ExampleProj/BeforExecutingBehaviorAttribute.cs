using System;

namespace ExampleProj.MethodCallTracing
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public class BeforExecutingBehaviorAttribute : Attribute, IMethodBehaviorAttribute
	{
		public string ActionName { get; }

		///
		public BeforExecutingBehaviorAttribute(string ActionName)
		{
			this.ActionName = ActionName;
		}
	}
}