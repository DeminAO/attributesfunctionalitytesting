using System;
using System.Threading.Tasks;

namespace MethodCallTracing
{
	class Program
	{
		static async Task<int> Main()
		{
			var t = new ExampleClass();

			Act(t.Invoke);
			Act(() => t.Invoke(5));
			_ = Act(t.InvokeAsync);
			await Act(t.InvokeAsync);

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