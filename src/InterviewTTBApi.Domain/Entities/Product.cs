namespace InterviewTTBApi.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    private Product() { }

    public static Product Create(string name, decimal price, int stock) =>
        new() { Id = Guid.NewGuid(), Name = name, Price = price, Stock = stock };

    public void DeductStock(int quantity) => Stock -= quantity;
    public void RestoreStock(int quantity) => Stock += quantity;

    public void Update(string name, decimal price, int stock)
    {
        Name = name;
        Price = price;
        Stock = stock;
    }
}
