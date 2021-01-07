﻿using System;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Linq;
using System.Runtime.Remoting;
using System.Threading.Tasks;
using System.Collections;

namespace ExampleProj.MethodCallTracing
{
	internal sealed class CallTracingAspect : IMessageSink
	{
		private readonly object objectRef;
		private readonly Type type;
		
		public CallTracingAspect(IMessageSink next, Type type, MarshalByRefObject refToObj)
		{
			NextSink = next;
			this.type = type;
			objectRef = refToObj;
		}

		public IMessageSink NextSink { get; }

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
		{
			return NextSink.AsyncProcessMessage(msg, replySink);
		}

		public IMessage SyncProcessMessage(IMessage msg)
		{
			var t = msg.Properties["__MethodName"].ToString();
			var m = type.GetMethods()
				.Where(x => x.Name == t && x.GetParameters().Length == (msg.Properties["__MethodSignature"] as ICollection).Count)
				.FirstOrDefault();

			IMethodBehaviorAttribute attr = m?.GetCustomAttribute<BeforExecutingBehaviorAttribute>();
			Preprocess(attr);

			var result = NextSink.SyncProcessMessage(msg);

			attr = m?.GetCustomAttribute<AfterExecutingBehaviorAttribute>();
			Postprocess(attr);

			return result;
		}

		private void Preprocess(IMethodBehaviorAttribute attr)
			=> Behavoir(attr?.ActionName);

		private void Postprocess(IMethodBehaviorAttribute attr)
			=> Behavoir(attr?.ActionName);

		private async void Behavoir(string methodName)
		{
			if (string.IsNullOrWhiteSpace(methodName))
			{
				return;
			}

			var methods = type
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.Name == methodName && !x.GetParameters().Any())
				.ToList();

			if (!methods.Any())
			{
				return;
			}

			ObjRef obj = RemotingServices.Marshal(objectRef as MarshalByRefObject, null, type);
			var objToRef = Activator.GetObject(type, obj.URI);

			var action = methods.FirstOrDefault(x => x.ReturnType == typeof(void));
			if (action != null)
			{
				// put in array needed parameters 
				action.Invoke(objToRef, new object[] { });
			}
			else
			{
				action = methods.FirstOrDefault(x => x.ReturnType == typeof(Task));
				if (action != null)
				{
					// put in array needed parameters 
					await (Task)action.Invoke(objToRef, new object[] { });
				}
			}
		}
	}
}