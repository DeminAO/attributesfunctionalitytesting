using System;

namespace ExampleProj.MethodCallTracing
{
	public sealed class AfterExecutingBehaviorAttribute : MethodBehaviorAttribute
	{
		///
		public AfterExecutingBehaviorAttribute(string ActionName)
		{
			this.ActionName = ActionName;
		}
	}
}