using System;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Remoting;

namespace ExampleProj.MethodCallTracing
{
	internal sealed class CallTracingProperty : IContextProperty, IContributeObjectSink
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
		public IMessageSink GetObjectSink(MarshalByRefObject obj, IMessageSink next)
		{
			return !methodInfos.Any() ? next : new CallTracingAspect(next, type, obj);
		}

		///
		public bool IsNewContextOK(Context newCtx) => true;
	}
}