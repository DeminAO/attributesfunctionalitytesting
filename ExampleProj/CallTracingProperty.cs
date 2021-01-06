using System;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using System.Collections.Generic;

namespace ExampleProj.MethodCallTracing
{
	internal class CallTracingProperty : IContextProperty, IContributeObjectSink
	{
		public string Name { get; }
		private readonly Type type;
		private readonly IEnumerable<MethodInfo> methodInfos;

		public CallTracingProperty(string name, Type type, IEnumerable<MethodInfo> methodInfos)
		{
			Name = name;
			this.type = type;
			this.methodInfos = methodInfos ?? new List<MethodInfo>();
		}

		public void Freeze(Context newContext) { }

		///
		public IMessageSink GetObjectSink(MarshalByRefObject o, IMessageSink next)
		{
			return !methodInfos.Any() ? next : new CallTracingAspect(next, type, o);
		}

		///
		public bool IsNewContextOK(Context newCtx) => true;
	}
}