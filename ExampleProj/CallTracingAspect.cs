using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using System.Collections.Generic;

namespace ExampleProj.MethodCallTracing
{
	internal class CallTracingAspect : IMessageSink
	{
		private readonly Type type;
		private readonly IEnumerable<MethodInfo> methodInfos;

		public CallTracingAspect(IMessageSink next, Type type, IEnumerable<MethodInfo> methodInfos)
		{
			NextSink = next;
			this.type = type;
			this.methodInfos = methodInfos;
		}

		public IMessageSink NextSink { get; }

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			var t = msg.Properties["__MethodName"].ToString();

			var m = type.GetMethod(t);

			if (!methodInfos.Any(x => x == m))
			{
				return NextSink.AsyncProcessMessage(msg, replySink);
			}

			var attr = m.GetCustomAttributes<MethodTracingAttribute>().FirstOrDefault();

			Preprocess(attr);
			var result = NextSink.AsyncProcessMessage(msg, replySink);
			Postprocess(attr);

			return result;
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			var t = msg.Properties["__MethodName"].ToString();

			var m = type.GetMethod(t);

			if (!methodInfos.Any(x => x == m))
			{
				return NextSink.SyncProcessMessage(msg);
			}

			var attr = m.GetCustomAttributes<MethodTracingAttribute>().FirstOrDefault();

			Preprocess(attr);
			var result = NextSink.SyncProcessMessage(msg);
			Postprocess(attr);

			return result;
		}

		private void Preprocess(MethodTracingAttribute methodTracing)
		{
			// set us up in the call context
			Console.WriteLine($"{nameof(CallTracingAspect)} -> {nameof(Preprocess)}");
		}

		private void Postprocess(MethodTracingAttribute methodTracing)
		{
			// set us up in the call context
			Console.WriteLine($"{nameof(CallTracingAspect)} -> {nameof(Postprocess)}");
			// var afterAction = type.GetProperty(methodTracing.After, typeof(Action)).GetValue();
		}
	}
}