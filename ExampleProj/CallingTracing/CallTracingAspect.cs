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
		/// <summary>
		/// маршал текущего обрабатываемого объекта
		/// </summary>
		private MarshalByRefObject MarshalByRefObject { get; }

		/// <summary>
		/// тип текущего обрабатываемого объекта
		/// </summary>
		private Type ObjectType { get; }

		/// <inheritdoc cref="IMessageSink.NextSink"/>
		public IMessageSink NextSink { get; }

		/// <param name="next">Следующий процесс в цепочке вызовов</param>
		/// <param name="type">Тип текущего обрабатываемого объекта</param>
		/// <param name="marshalByRefObject">Маршал текущего обрабатываемого объекта</param>
		public CallTracingAspect(IMessageSink next, Type type, MarshalByRefObject marshalByRefObject)
		{
			NextSink = next;
			this.ObjectType = type;
			MarshalByRefObject = marshalByRefObject;
		}

		/// <inheritdoc cref="IMessageSink.AsyncProcessMessage(IMessage, IMessageSink)"/>
		public IMessageCtrl AsyncProcessMessage(IMessage msg, IMessageSink replySink)
			=> ProcessNextSink(msg, () => NextSink.AsyncProcessMessage(msg, replySink));

		/// <inheritdoc cref="IMessageSink.SyncProcessMessage(IMessage)"/>
		public IMessage SyncProcessMessage(IMessage msg)
			=> ProcessNextSink(msg, () => NextSink.SyncProcessMessage(msg));

		/// <summary>
		/// Метод, управлчющий блоком синхронизации текущего вызванного метода. 
		/// Достает аттрибуты MethodBehaviorAttribute текущег метода и обрабатывает метод в соответствии с ними
		/// </summary>
		/// <typeparam name="T">IMessage или IMessageCtrl</typeparam>
		/// <param name="msg">сообщение, необходимое следующему процессу</param>
		/// <param name="getSinc">Метод, возвращаующий процесс длч следующего метода в цепоке вызовов</param>
		private T ProcessNextSink<T>(IMessage msg, Func<T> getSinc)
		{
			// наименование текущего вызываемого метода
			var t = msg.Properties["__MethodName"].ToString();

			// информацич о текущем методе
			var methodInfo = ObjectType.GetMethods()
				.Where(x => x.Name == t && x.GetParameters().Length == (msg.Properties["__MethodSignature"] as ICollection).Count)
				.FirstOrDefault();

			// объект, метод которого обрабатываетсч
			ObjRef objRef = RemotingServices.Marshal(MarshalByRefObject, null, ObjectType);
			var obj = Activator.GetObject(ObjectType, objRef.URI);

			// обработка навешенных атрибутов

			// операции до вызова метода

			// атрибут метода BinaryIndicator
			var binInd = methodInfo?.GetCustomAttribute<BinaryIndicatorAttribute>();
			SetValueToObjectProperty(binInd?.ActionName, obj, true);

			// атрибут метода BeforExecutingBehavior
			ActionNameAttribute attr = methodInfo?.GetCustomAttribute<BeforExecutingBehaviorAttribute>();
			BeforAndAfterBehavoir(attr?.ActionName, obj).GetAwaiter().GetResult();


			// вызов метода
			var result = getSinc.Invoke();


			// операции после вызова метода

			// атрибут метода AfterExecutingBehavior
			attr = methodInfo?.GetCustomAttribute<AfterExecutingBehaviorAttribute>();
			BeforAndAfterBehavoir(attr?.ActionName, obj).GetAwaiter().GetResult();

			// атрибут метода BinaryIndicator
			SetValueToObjectProperty(binInd?.ActionName, obj, false);

			// возврат вызова метода
			return result;
		}

		/// <summary>
		/// Установка свойству указанного объекта необходимого значенич
		/// </summary>
		/// <param name="propName">Наименование метода</param>
		/// <param name="obj">Объект длч установки значенич</param>
		/// <param name="value">Значение длч установки в свойство (типы должны совпадать)</param>
		private void SetValueToObjectProperty(string propName, object obj, object value)
		{
			if (string.IsNullOrWhiteSpace(propName))
			{
				return;
			}
			
			ObjectType
				.GetProperty(propName, BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.Instance)
				?.SetValue(obj, value);
		}

		/// <summary>
		/// Обработик атрибутов BeforExecutingBehaviorAttribute и AfterExecutingBehaviorAttribute.
		/// Вызывает указанный метод. Поддерживаетсч ассинхронность
		/// </summary>
		/// <param name="methodName">Наименование метода</param>
		/// <param name="obj">Объект, у которого необходимо вызвать указанный метод</param>
		private async Task BeforAndAfterBehavoir(string methodName, object obj)
		{
			if (string.IsNullOrWhiteSpace(methodName))
			{
				return;
			}

			// методы, без параметров, совпадающие по наименованию с необходимым методом.
			// неоптимизировано - такой метод может быть только один по конвенции. 
			// Оставил так длч дальнейших доработок, возможности отправки параметров
			var methods = ObjectType
				.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
				.Where(x => x.Name == methodName && !x.GetParameters().Any());

			if (!methods.Any())
			{
				return;
			}

			// метод без возврата обрабатываем в текущем потоке
			var action = methods.FirstOrDefault(x => x.ReturnType == typeof(void));
			if (action != null)
			{
				// put in array needed parameters
				action.Invoke(obj, new object[] { });
				return;
			}
			
			// Таски обрабатываем в отдельном потоке
			action = methods.FirstOrDefault(x => x.ReturnType == typeof(Task));
			if (action != null)
			{
				// выглчдит как извращение, но, так работает
				// put in array needed parameters
				await Task.Run(async () => await (Task)action.Invoke(obj, new object[] { }));
			}
		}
	}
}