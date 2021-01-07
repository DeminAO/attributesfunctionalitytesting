namespace ExampleProj.MethodCallTracing
{
	/// <summary>
	/// Задает значение true длч булевого свойства до вызова метода и false - после вызова метода
	/// </summary>
	public sealed class BinaryIndicatorAttribute : ActionNameAttribute
	{
		/// <param name="PropertyName">Наименование булевого свойства</param>
		public BinaryIndicatorAttribute(string PropertyName)
		{
			ActionName = PropertyName;
		}
	}
}