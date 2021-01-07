using ExampleProj.MethodCallTracing;
using System;
using System.Threading.Tasks;
using ExampleProj.Interfaces;

namespace MethodCallTracing
{
	[CallTracing]
	public class ExampleClassBase : ContextBoundObject, IMethodTracing
	{
		public void Invoke()
		{
			Console.WriteLine($"{nameof(ExampleClassBase)} -> {nameof(Invoke)}");
		}

		[BeforExecutingBehavior(ActionName: nameof(BeforHandlerAsync))]
		[AfterExecutingBehavior(ActionName: nameof(AfterHandlerAsync))]
		public void Invoke(int i)
		{
			Console.WriteLine($"{nameof(ExampleClassBase)} -> {nameof(Invoke)}({i})");
			// throw new Exception();
		}

		[AfterExecutingBehavior(ActionName: nameof(AfterHandlerAsync))]
		public async Task InvokeAsync()
		{
			Console.WriteLine($"{nameof(ExampleClassBase)} -> {nameof(InvokeAsync)}");
			await Task.CompletedTask;
			// throw new Exception();
		}

		private async Task BeforHandlerAsync()
		{
			Console.WriteLine($"{nameof(ExampleClassBase)} -> {nameof(BeforHandlerAsync)}");
			await Task.CompletedTask;
		}
		
		public virtual async Task AfterHandlerAsync()
		{
			Console.WriteLine($"{nameof(ExampleClassBase)} -> {nameof(AfterHandlerAsync)}");
			await Task.CompletedTask;
		}
	}
}