namespace SingletonIdGenerator;

public sealed class IdGenerator
{
    // Единственный экземпляр с ленивой инициализацией
    private static readonly Lazy<IdGenerator> _instance = new(() => new IdGenerator());

    // Счётчик ID
    private long _counter = 0;

    // Закрытый конструктор
    private IdGenerator() { }

    // Метод получения экземпляра (потокобезопасный)
    public static IdGenerator GetInstance() => _instance.Value;

    // Генерация уникального ID
    public long NextId() => Interlocked.Increment(ref _counter);

    // Генерация ID с префиксом
    public string NextId(string prefix) => $"{prefix}-{NextId()}";
}

class Program
{
    static void Main()
    {
        var generator1 = IdGenerator.GetInstance();
        var generator2 = IdGenerator.GetInstance();

        Console.WriteLine("Проверка Singleton:");
        Console.WriteLine(ReferenceEquals(generator1, generator2));

        Console.WriteLine("\nГенерация ID:");
        Console.WriteLine(generator1.NextId());
        Console.WriteLine(generator2.NextId());
        Console.WriteLine(generator1.NextId("USR"));
        Console.WriteLine(generator2.NextId("ORD"));

        Console.ReadKey();
    }
}
