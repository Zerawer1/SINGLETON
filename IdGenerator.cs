using System;
using System.Threading;

namespace SingletonIdGenerator
{
    public sealed class IdGenerator
    {
        // Единственный экземпляр
        private static IdGenerator _instance;

        // Объект для блокировки
        private static readonly object _lock = new object();

        // Счётчик ID
        private long _counter = 0;

        // Закрытый конструктор
        private IdGenerator() { }

        // Метод получения экземпляра (потокобезопасный)
        public static IdGenerator GetInstance()
        {
            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new IdGenerator();
                }
            }

            return _instance;
        }

        // Генерация уникального ID
        public long NextId()
        {
            return Interlocked.Increment(ref _counter);
        }

        // Генерация ID с префиксом
        public string NextId(string prefix)
        {
            return $"{prefix}-{NextId()}";
        }
    }

    class Program
    {
        static void Main()
        {
            var generator1 = IdGenerator.GetInstance();
            var generator2 = IdGenerator.GetInstance();

            Console.WriteLine("Проверка Singleton:");
            Console.WriteLine(Object.ReferenceEquals(generator1, generator2));

            Console.WriteLine("\nГенерация ID:");
            Console.WriteLine(generator1.NextId());
            Console.WriteLine(generator2.NextId());
            Console.WriteLine(generator1.NextId("USR"));
            Console.WriteLine(generator2.NextId("ORD"));

            Console.ReadKey();
        }
    }
}
