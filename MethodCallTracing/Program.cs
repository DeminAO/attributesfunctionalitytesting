using System;
using System.Threading.Tasks;

namespace MethodCallTracing
{
	class Program
	{
		static async Task<int> Main()
		{

			Console.WriteLine(nameof(ExampleClassBase) + " test:" );

			var t = new ExampleClassBase();

			Act(t.Invoke);
			Act(() => t.Invoke(5));
			await Act(t.InvokeAsync);
			
			Console.WriteLine(nameof(ExampleExtendedClass) + " test:" );

			var t2 = new ExampleExtendedClass();

			Act(() => t2.Invoke(5));
			await Act(t2.InvokeAsync);

			Console.ReadKey(false);

			return 0;
		}

		private static void Act(Action action)
		{
			try
			{
				action.Invoke();
			}
			catch (Exception e)
			{
				e.ToString();
			}
			Console.WriteLine("-------------");
		}

		private static async Task Act(Func<Task> action)
		{
			try
			{
				await action.Invoke();
			}
			catch
			{

			}
			Console.WriteLine("-------------");
		}
	}
}