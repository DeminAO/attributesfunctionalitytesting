using ExampleProj.MethodCallTracing;
using System;
using System.Threading.Tasks;
using ExampleProj.Interfaces;

namespace MethodCallTracing
{
	[CallTracing]
	public class ExampleClass : ContextBoundObject, IMethodTracing
	{
		public void Invoke()
		{
			Console.WriteLine($"{nameof(ExampleClass)} -> {nameof(Invoke)}");
		}

		[BeforExecutingBehavior(ActionName: nameof(BeforHandler))]
		[AfterExecutingBehavior(ActionName: nameof(AfterHandlerAsync))]
		public void Invoke(int i)
		{
			Console.WriteLine($"{nameof(ExampleClass)} -> {nameof(Invoke)}({i})");
			throw new Exception();
		}

		[BeforExecutingBehavior(ActionName: nameof(BeforHandler))]
		public async Task InvokeAsync()
		{
			Console.WriteLine($"{nameof(ExampleClass)} -> {nameof(InvokeAsync)}");
			await Task.CompletedTask;
			throw new Exception();
		}

		private void BeforHandler()
		{
			Console.WriteLine($"{nameof(ExampleClass)} -> {nameof(BeforHandler)}");
		}
		
		private async Task AfterHandlerAsync()
		{
			Console.WriteLine($"{nameof(ExampleClass)} -> {nameof(AfterHandlerAsync)}");
			await Task.CompletedTask;
		}
	}
}