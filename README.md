# attributesfunctionalitytesting


В данном проекте демонстрируется использование атрибутов для методов.
Передо мной стояла задача, реализовав вызов методов через атрибуты, превратить "Код 1" в "Код 2" (с учетом того, что Exception\`ы ловятcя в middleware). 


```
private async Task PostRequestAsync()
{
  IsLoading = true;
  try
  {
    SomeProperty = await Repository.LoadAsync();
  }
  finaly
  {
    IsLoading = false;
  }
}
```

```
[BinaryIndicator(PropertyName = nameof(IsLoading))]
private async Task PostRequestAsync()
{
    SomeProperty = await Repository.LoadAsync();
}
```

Но, это выглядит опрятней ценой производительности. я, правда, расчеты не делал, но, могу точно сказать, что 6 строк с try-finaly будут работать в сотню раз быстрее, 
чем косарь строк с использованием рефлексии. При работе консольки даже видно, как она начинает подтупливать.
