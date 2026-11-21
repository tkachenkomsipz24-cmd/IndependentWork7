using IndependentWork7;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Cache<string, string> (ємність 3) ===");
var cache = new Cache<string, string>(3);

cache.Add("a", "Apple");
cache.Add("b", "Banana");
cache.Add("c", "Cherry");
Console.WriteLine($"Додано a, b, c. Count = {cache.Count}");
Console.WriteLine($"Get(\"a\") = {cache.Get("a")}");

cache.Add("d", "Date");
Console.WriteLine("Додано d (кеш переповнено).");
Console.WriteLine($"Count = {cache.Count}");
Console.WriteLine($"ContainsKey(\"a\") = {cache.ContainsKey("a")}  (найстарший видалено)");
Console.WriteLine($"ContainsKey(\"d\") = {cache.ContainsKey("d")}");

try
{
    cache.Get("a");
}
catch (KeyNotFoundException ex)
{
    Console.WriteLine($"KeyNotFoundException: {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("=== Cache<int, MyObject> (ємність 2) ===");
var objects = new Cache<int, MyObject>(2);

objects.Add(1, new MyObject("Перший"));
objects.Add(2, new MyObject("Другий"));
objects.Add(3, new MyObject("Третій")); // видаляється ключ 1

for (int id = 1; id <= 3; id++)
    Console.WriteLine($"Ключ {id}: {(objects.ContainsKey(id) ? objects.Get(id).ToString() : "відсутній")}");

Console.WriteLine($"Count = {objects.Count}");

public record MyObject(string Name);
