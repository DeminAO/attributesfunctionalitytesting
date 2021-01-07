using System;

namespace ExampleProj.MethodCallTracing
{
	/// <summary>
	/// Назначает метод, выполнчющийсч до вызова метода, на который данный атрибут повешен
	/// </summary>
	public class BeforExecutingBehaviorAttribute : ActionNameAttribute
	{
		/// <param name="ActionName">Наименование метода без параметров длч вызова</param>
		public BeforExecutingBehaviorAttribute(string ActionName)
		{
			this.ActionName = ActionName;
		}
	}
}