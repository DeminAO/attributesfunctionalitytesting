using ExampleProj.MethodCallTracing;
using System;
using System.Threading.Tasks;

namespace MethodCallTracing
{
	// [CallTracing]
	public class ExampeClass : ContextBoundObject
	{
		public Action Before => BeforHandler;
		public Action After => AfterHandler;

		public void Invoke()
		{
			Console.WriteLine($"{nameof(ExampeClass)} -> {nameof(Invoke)}");
			throw new Exception();
		}

		[MethodTracing(beforeActionName: nameof(Before), afterActionName: nameof(After))]
		public void Invoke(int i)
		{
			Console.WriteLine($"{nameof(ExampeClass)} -> {nameof(Invoke)}({i})");
			throw new Exception();
		}

		[MethodTracing(beforeActionName: nameof(Before), afterActionName: nameof(After))]
		public async Task InvokeAsync()
		{
			Console.WriteLine($"{nameof(ExampeClass)} -> {nameof(InvokeAsync)}");
			await Task.CompletedTask;
			throw new Exception();
		}

		private void BeforHandler()
		{
			Console.WriteLine($"{nameof(ExampeClass)} -> {nameof(Before)}");
			throw new Exception();
		}
		
		private void AfterHandler()
		{
			Console.WriteLine($"{nameof(ExampeClass)} -> {nameof(After)}");
		}
	}
}