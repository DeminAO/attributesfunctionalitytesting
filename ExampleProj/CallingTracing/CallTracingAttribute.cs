using System;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Linq;

namespace ExampleProj.MethodCallTracing
{
	///
	[AttributeUsage(AttributeTargets.Class, Inherited = true)]
	public sealed class CallTracingAttribute : ContextAttribute
	{
		private const string callTrace = "CallTrace";
		///
		public CallTracingAttribute() : base(callTrace) { }

		///
		public override void GetPropertiesForNewContext(IConstructionCallMessage ccm)
		{
			var type = Type.GetType(ccm.TypeName);
			
			var methodsInfos = type
				.GetMethods()
				.Where(x => x.GetCustomAttributes<ActionNameAttribute>().Any());

			ccm.ContextProperties.Add(new CallTracingProperty(callTrace, type, methodsInfos));
		}
	}
}