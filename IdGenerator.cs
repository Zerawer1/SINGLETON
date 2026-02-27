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
