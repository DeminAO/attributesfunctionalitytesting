using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;

namespace ExampleProj.MethodCallTracing
{
	[AttributeUsage(AttributeTargets.Method)]
	public class MethodTracingAttribute : ContextAttribute
	{
		public string Before { get; }
		public string After { get; }

		///
		public MethodTracingAttribute(string beforeActionName, string afterActionName) : base("MethodTracingAttribute")
		{
			Console.WriteLine($"{nameof(MethodTracingAttribute)} -> ctor");
			Before = beforeActionName;
			After = afterActionName;
		}

		///
		public override void GetPropertiesForNewContext(IConstructionCallMessage ccm)
		{
			var type = Type.GetType(ccm.TypeName);

			var methodsInfos = type
				.GetMethods()
				.Where(x => x.GetCustomAttribute<MethodTracingAttribute>() != null);

			if (methodsInfos.Any())
			{
				ccm.ContextProperties.Add(new CallTracingProperty("MethodTracingAttribute", type, methodsInfos));
			}
		}
	}
}