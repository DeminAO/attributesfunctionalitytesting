using System;

namespace ExampleProj.MethodCallTracing
{
	public class BeforExecutingBehaviorAttribute : MethodBehaviorAttribute
	{
		///
		public BeforExecutingBehaviorAttribute(string ActionName)
		{
			this.ActionName = ActionName;
		}
	}
}