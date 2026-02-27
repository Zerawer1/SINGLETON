using SingletonIdGenerator.Services;

namespace SingletonIdGenerator;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Демонстрация Singleton IdGenerator ===\n");

        // Создаем сервисы, которые используют один и тот же IdGenerator
        var userService = new UserService();
        var orderService = new OrderService();

        // Проверяем, что оба сервиса используют один экземпляр IdGenerator
        var generatorFromUserService = IdGenerator.GetInstance();
        var generatorFromOrderService = IdGenerator.GetInstance();
        
        Console.WriteLine($"Оба сервиса используют один IdGenerator: {ReferenceEquals(generatorFromUserService, generatorFromOrderService)}\n");

        // Демонстрация работы UserService
        Console.WriteLine("--- Работа с пользователями ---");
        var userResults = userService.CreateMultipleUsers("Alice", "Bob", "Charlie");
        Console.WriteLine(userResults);
        Console.WriteLine();

        // Демонстрация работы OrderService
        Console.WriteLine("--- Работа с заказами ---");
        var orderResults = orderService.CreateMultipleOrders(
            ("Laptop", 1299.99m),
            ("Mouse", 29.99m),
            ("Keyboard", 79.99m)
        );
        Console.WriteLine(orderResults);
        Console.WriteLine();

        // Демонстрация смешанной генерации ID
        Console.WriteLine("--- Смешанная генерация ID ---");
        Console.WriteLine(userService.CreateUser("David"));
        Console.WriteLine(orderService.CreateOrder("Monitor", 299.99m));
        Console.WriteLine(userService.CreateUser("Eve"));

        Console.WriteLine("\nНажмите любую клавишу для выхода...");
        Console.ReadKey();
    }
}
