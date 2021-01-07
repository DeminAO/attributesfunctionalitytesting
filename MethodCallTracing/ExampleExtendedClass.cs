using ExampleProj.MethodCallTracing;
using System;
using System.Threading.Tasks;

namespace MethodCallTracing
{
	[CallTracing]
	public class ExampleExtendedClass : ExampleClassBase
	{
		[BeforExecutingBehavior(nameof(BeforAfterHandler))]
		[AfterExecutingBehavior(nameof(AfterAfterHandlerAsync))]
		public override Task AfterHandlerAsync()
		{
			return base.AfterHandlerAsync();
		}

		private void BeforAfterHandler()
		{
			Console.WriteLine($"{nameof(ExampleExtendedClass)} -> {nameof(BeforAfterHandler)}");
		}

		private async Task AfterAfterHandlerAsync()
		{
			Console.WriteLine($"{nameof(ExampleExtendedClass)} -> {nameof(AfterAfterHandlerAsync)}");
			await Task.CompletedTask;
		}
	}
}