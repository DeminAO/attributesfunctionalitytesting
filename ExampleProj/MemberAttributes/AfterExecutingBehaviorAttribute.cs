using System;

namespace ExampleProj.MethodCallTracing
{
	/// <summary>
	/// Назначает метод, выполнчющийсч после вызова метода, на который данный атрибут повешен
	/// </summary>
	public sealed class AfterExecutingBehaviorAttribute : ActionNameAttribute
	{
		/// <param name="ActionName">Наименование метода без параметров длч вызова</param>
		public AfterExecutingBehaviorAttribute(string ActionName)
		{
			this.ActionName = ActionName;
		}
	}
}