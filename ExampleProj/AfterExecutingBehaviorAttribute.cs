using System;

namespace ExampleProj.MethodCallTracing
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public sealed class AfterExecutingBehaviorAttribute : Attribute, IMethodBehaviorAttribute
	{
		public string ActionName { get; }

		///
		public AfterExecutingBehaviorAttribute(string ActionName)
		{
			this.ActionName = ActionName;
		}
	}
}