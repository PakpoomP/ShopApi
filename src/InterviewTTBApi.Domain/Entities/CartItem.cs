namespace InterviewTTBApi.Domain.Entities;

public class CartItem
{
    public Guid Id { get; private set; }
    public string CartId { get; private set; }
    public Guid ProductId { get; private set; }
    public int Quantity { get; private set; }
    public bool Status { get; private set; }

    private CartItem() { }

    public static CartItem Create(string cartId, Guid productId, int quantity) =>
        new() { Id = Guid.NewGuid(), CartId = cartId, ProductId = productId, Quantity = quantity };

    public void AddQuantity(int quantity) => Quantity += quantity;
    public void SetQuantity(int quantity) => Quantity = quantity;
    public void Checkout() => Status = true;
}
