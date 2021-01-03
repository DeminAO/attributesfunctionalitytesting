using System;
using System.Threading.Tasks;

namespace MethodCallTracing
{
	class Program
	{
		static async Task<int> Main(string[] args)
		{
			var t = new ExampeClass();
			Console.WriteLine("-------------");

			try
			{
				t.Invoke();
			}
			catch
			{

			}
			Console.WriteLine("-------------");
			
			try
			{
				t.Invoke(5);
			}
			catch
			{

			}
			Console.WriteLine("-------------");

			try
			{
				_ = t.InvokeAsync();
			}
			catch
			{

			}
			Console.WriteLine("-------------");

			try
			{
				await t.InvokeAsync();
			}
			catch
			{

			}
			Console.WriteLine("-------------");

			Console.ReadKey(false);

			return 0;
		}
	}
}