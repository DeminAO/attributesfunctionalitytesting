using System;

namespace ExampleProj.MethodCallTracing
{
	/// <summary>
	/// Абстрактный базовый класс длч всех атрибутов, используемых в CallingTracing
	/// </summary>
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
	public abstract class ActionNameAttribute : Attribute
	{
		/// <summary>
		/// Наименование метода
		/// </summary>
		public string ActionName { get; protected set; }
	}
}