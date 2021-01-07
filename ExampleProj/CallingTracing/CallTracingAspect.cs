using System;
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
		private MarshalByRefObject MarshalByRefObject { get; }
		private Type ObjectType { get; }
		
		public CallTracingAspect(IMessageSink next, Type type, MarshalByRefObject marshalByRefObject)
		{
			NextSink = next;
			this.ObjectType = type;
			MarshalByRefObject = marshalByRefObject;
		}

		public IMessageSink NextSink { get; }

		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
			=> ProcessNextSink(msg, () => NextSink.AsyncProcessMessage(msg, replySink));

		public IMessage SyncProcessMessage(IMessage msg)
			=> ProcessNextSink(msg, () => NextSink.SyncProcessMessage(msg));

		private T ProcessNextSink<T>(IMessage msg, Func<T> getSinc)
		{
			var t = msg.Properties["__MethodName"].ToString();
			var m = ObjectType.GetMethods()
				.Where(x => x.Name == t && x.GetParameters().Length == (msg.Properties["__MethodSignature"] as ICollection).Count)
				.FirstOrDefault();

			ObjRef objRef = RemotingServices.Marshal(MarshalByRefObject, null, ObjectType);
			var obj = Activator.GetObject(ObjectType, objRef.URI);

			MethodBehaviorAttribute attr = m?.GetCustomAttribute<BeforExecutingBehaviorAttribute>();
			BeforAndAfterBehavoir(attr?.ActionName, obj).GetAwaiter().GetResult();

			var result = getSinc.Invoke();

			attr = m?.GetCustomAttribute<AfterExecutingBehaviorAttribute>();
			BeforAndAfterBehavoir(attr?.ActionName, obj).GetAwaiter().GetResult();

			return result;
		}

		private async Task BeforAndAfterBehavoir(string methodName, object obj)
		{
			if (string.IsNullOrWhiteSpace(methodName))
			{
				return;
			}

			var methods = ObjectType
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.Name == methodName && !x.GetParameters().Any());

			if (!methods.Any())
			{
				return;
			}

			var action = methods.FirstOrDefault(x => x.ReturnType == typeof(void));
			if (action != null)
			{
				// put in array needed parameters 
				action.Invoke(obj, new object[] { });
			}
			else
			{
				action = methods.FirstOrDefault(x => x.ReturnType == typeof(Task));
				if (action != null)
				{
					// put in array needed parameters 
					await Task.Run(async () => await (Task)action.Invoke(obj, new object[] { }));
				}
			}
		}
	}
}