# SingletonIdGenerator

Консольное приложение на C# (.NET 9), демонстрирующее использование паттерна Singleton для генерации уникальных ID в многокомпонентной системе.

Программа создаёт один общий генератор ID, который:

* выдаёт уникальные числовые значения (`1`, `2`, `3`, …)
* формирует ID с префиксом (`USR-1`, `ORD-2`, …)
* работает корректно в многопоточном режиме

---

## Идея проекта

Главная задача — реализовать единый источник генерации ID в рамках одного процесса.

Если бы можно было создать несколько экземпляров генератора, каждый начал бы считать с `1`, что привело бы к дубликатам.
Паттерн **Singleton** гарантирует существование только одного экземпляра класса.

---

## Реализация Singleton

Singleton реализован в классе `IdGenerator`.

### 1. Единственный экземпляр

```csharp
private static IdGenerator _instance;
```

Статическое поле хранит ссылку на единственный объект.

### Что такое Singleton?

**Singleton** - это порождающий паттерн проектирования, который гарантирует, что у класса есть только один экземпляр, и предоставляет глобальную точку доступа к этому экземпляру.

### Где используется Singleton в проекте

Singleton реализован в классе `IdGenerator`:

```csharp
public sealed class IdGenerator
{
    // Единственный экземпляр с ленивой инициализацией
    private static readonly Lazy<IdGenerator> _instance = new(() => new IdGenerator());
    
    // Закрытый конструктор - предотвращает создание извне
    private IdGenerator() { }
    
    // Глобальная точка доступа
    public static IdGenerator GetInstance() => _instance.Value;
}
```

### Как работает реализация

1. **`Lazy<IdGenerator>`** - обеспечивает ленивую и потокобезопасную инициализацию
2. **Закрытый конструктор** - предотвращает создание экземпляров через `new`
3. **`GetInstance()`** - единственный способ получить доступ к экземпляру

### Потокобезопасность

- **`Lazy<T>`** автоматически обеспечивает потокобезопасность создания
- **`Interlocked.Increment()`** атомарно увеличивает счётчик без блокировок

## Компоненты системы

### 1. IdGenerator (Singleton)

**Ответственность:** Генерация уникальных числовых ID и ID с префиксом.

```csharp
public long NextId()                    // -> 1, 2, 3, ...
public string NextId(string prefix)     // -> "USR-1", "ORD-2", ...
```

**Ключевые особенности:**
- Чистый класс без зависимостей от консоли
- Потокобезопасный
- Единственный экземпляр на всё приложение

### 2. UserService

**Ответственность:** Управление пользователями, использует `IdGenerator` для генерации ID пользователей.

```csharp
public string CreateUser(string username)
{
    var userId = _idGenerator.NextId("USR");
    return $"Создан пользователь: {username} с ID: {userId}";
}
```

### 3. OrderService

**Ответственность:** Управление заказами, использует тот же `IdGenerator` для генерации ID заказов.

```csharp
public string CreateOrder(string productName, decimal amount)
{
    var orderId = _idGenerator.NextId("ORD");
    return $"Создан заказ: {productName} на сумму {amount:C} с ID: {orderId}";
}
```

## Почему здесь выбран Singleton?

### Проблема без Singleton

Если бы каждый сервис создавал свой `IdGenerator`:

```csharp
// Плохой подход - разные счётчики
var userService = new UserService(new IdGenerator());    // счётчик: 1, 2, 3...
var orderService = new OrderService(new IdGenerator());  // счётчик: 1, 2, 3...
```

**Результат:** Возможны дубликаты ID (USR-1 и ORD-1).

### Решение с Singleton

```csharp
// Правильный подход - общий счётчик
var userService = new UserService();     // использует IdGenerator.GetInstance()
var orderService = new OrderService();   // использует тот же IdGenerator.GetInstance()
```

**Результат:** Единая последовательность ID (USR-1, ORD-2, USR-3, ORD-4...).

### Преимущества в этом контексте

1. **Уникальность ID** - все сервисы используют один счётчик
2. **Экономия памяти** - только один экземпляр генератора
3. **Централизованное управление** - легко изменить логику генерации
4. **Глобальная доступность** - любой класс может получить доступ

## Демонстрация работы

### Пример выполнения

```csharp
=== Демонстрация Singleton IdGenerator ===

Оба сервиса используют один IdGenerator: True

--- Работа с пользователями ---
Создан пользователь: Alice с ID: USR-1
Создан пользователь: Bob с ID: USR-2
Создан пользователь: Charlie с ID: USR-3

--- Работа с заказами ---
Создан заказ: Laptop на сумму $1,299.99 с ID: ORD-4
Создан заказ: Mouse на сумму $29.99 с ID: ORD-5
Создан заказ: Keyboard на сумму $79.99 с ID: ORD-6

--- Смешанная генерация ID ---
Создан пользователь: David с ID: USR-7
Создан заказ: Monitor на сумму $299.99 с ID: ORD-8
Создан пользователь: Eve с ID: USR-9
```

### Анализ результатов

- **Единая последовательность:** ID продолжают общий счётчик (1, 2, 3, ...)
- **Разные префиксы:** USR для пользователей, ORD для заказов
- **Нет дубликатов:** Невозможно получить одинаковый ID

## Технологические особенности

### .NET 9 Modern C#

- **File-scoped namespaces** - `namespace SingletonIdGenerator;`
- **Target-typed new** - `new(() => new IdGenerator())`
- **Expression-bodied members** - `public static IdGenerator GetInstance() => _instance.Value;`
- **Implicit usings** - не нужны `using System;`
- **Nullable reference types** - включены для безопасности

### Потокобезопасность

```csharp
// Атомарная операция без блокировок
public long NextId() => Interlocked.Increment(ref _counter);

// Потокобезопасная ленивая инициализация
private static readonly Lazy<IdGenerator> _instance = new(() => new IdGenerator());
```

## Запуск проекта

```bash
# Клонирование репозитория
git clone https://github.com/Zerawer1/SINGLETON.git
cd SINGLETON

# Запуск
dotnet run
```

## Расширение проекта

### Добавление нового сервиса

```csharp
public class ProductService
{
    private readonly IdGenerator _idGenerator = IdGenerator.GetInstance();
    
    public string CreateProduct(string name)
    {
        var productId = _idGenerator.NextId("PRD");
        return $"Создан продукт: {name} с ID: {productId}";
    }
}
```

### Изменение логики генерации

Достаточно изменить класс `IdGenerator`, и все сервисы автоматически получат новую логику.

## Выводы

Этот проект демонстрирует правильное использование паттерна Singleton:

1. **Реальная потребность** - нужен единый источник ID
2. **Правильная реализация** - потокобезопасная, ленивая инициализация
3. **Чистая архитектура** - разделение ответственностей
4. **Практическое применение** - несколько классов используют один Singleton

Singleton здесь не просто "паттерн ради паттерна", а решение реальной проблемы обеспечения уникальности ID в распределённой системе.

Пример вывода программы:

```
Проверка Singleton:
True

Генерация ID:
1
2
USR-3
ORD-4
```

---

Проект демонстрирует корректную реализацию паттерна Singleton, потокобезопасное создание экземпляра и работу единого счётчика идентификаторов в рамках одного процесса.
