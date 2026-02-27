namespace SingletonIdGenerator.Services;

public class OrderService
{
    private readonly IdGenerator _idGenerator;

    public OrderService()
    {
        _idGenerator = IdGenerator.GetInstance();
    }

    public string CreateOrder(string productName, decimal amount)
    {
        var orderId = _idGenerator.NextId("ORD");
        return $"Создан заказ: {productName} на сумму {amount:C} с ID: {orderId}";
    }

    public string CreateMultipleOrders(params (string product, decimal amount)[] orders)
    {
        var results = new List<string>();
        foreach (var (product, amount) in orders)
        {
            results.Add(CreateOrder(product, amount));
        }
        return string.Join("\n", results);
    }
}
