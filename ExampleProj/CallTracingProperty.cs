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
			this.methodInfos = methodInfos;
		}

		public void Freeze(Context newContext) { }

		///
		public IMessageSink GetObjectSink(MarshalByRefObject o, IMessageSink next)
		{
			if (!methodInfos?.Any() ?? false)
			{
				return next;
			}

			return new CallTracingAspect(next, type, methodInfos);
		}

		///
		public bool IsNewContextOK(Context newCtx) => true;
	}
}