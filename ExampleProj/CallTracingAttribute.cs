using System;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Linq;

namespace ExampleProj.MethodCallTracing
{
	///
	[AttributeUsage(AttributeTargets.Class)]
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
				.Where(x => x.GetCustomAttribute<MethodTracingAttribute>() != null);

			if (methodsInfos.Any())
			{
				ccm.ContextProperties.Add(new CallTracingProperty(callTrace, type, methodsInfos));
			}
		}
	}
}