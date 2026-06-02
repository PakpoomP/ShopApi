using InterviewTTBApi.Application.Features.Cart.Commands;
using InterviewTTBApi.Application.Interfaces;
using InterviewTTBApi.Application.UnitTests.Helpers;
using InterviewTTBApi.Domain.Entities;
using InterviewTTBApi.Domain.Interfaces;
using NSubstitute;

namespace InterviewTTBApi.Application.UnitTests.Features.Cart;

public class AddToCartCommandHandlerTests
{
    private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
    private readonly IApplicationDbContext _db = Substitute.For<IApplicationDbContext>();
    private readonly List<CartItem> _cartItems = [];
    private readonly AddToCartCommandHandler _handler;

    public AddToCartCommandHandlerTests()
    {
        var cartItemsDbSet = DbSetMockHelper.CreateMockDbSet(_cartItems);
        _db.CartItems.Returns(cartItemsDbSet);
        _db.SaveChangesAsync(Arg.Any<CancellationToken>()).Returns(1);
        _handler = new AddToCartCommandHandler(_db, _productRepository);
    }

    [Fact]
    public async Task Handle_NewItem_CreatesCartItemAndDeductsStock()
    {
        var cartId = Guid.NewGuid().ToString();
        var product = Product.Create("Widget", 9.99m, 50);
        _productRepository.GetByIdAsync(product.Id).Returns(product);

        var result = await _handler.Handle(new AddToCartCommand(cartId, product.Id, 3), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(47, product.Stock);
        Assert.Single(_cartItems, c => c.CartId == cartId && c.ProductId == product.Id && c.Quantity == 3);
    }

    [Fact]
    public async Task Handle_ExistingItem_UpdatesQuantityAndDeductsStock()
    {
        var cartId = Guid.NewGuid().ToString();
        var product = Product.Create("Widget", 9.99m, 50);
        var existingItem = CartItem.Create(cartId, product.Id, 2);
        _cartItems.Add(existingItem);
        _productRepository.GetByIdAsync(product.Id).Returns(product);

        var result = await _handler.Handle(new AddToCartCommand(cartId, product.Id, 3), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Equal(47, product.Stock);
        Assert.Equal(5, existingItem.Quantity);
    }

    [Fact]
    public async Task Handle_ProductNotFound_ReturnsFailure()
    {
        _productRepository.GetByIdAsync(Arg.Any<Guid>()).Returns((Product?)null);

        var result = await _handler.Handle(new AddToCartCommand(Guid.NewGuid().ToString(), Guid.NewGuid(), 1), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Product not found.", result.Error);
    }

    [Fact]
    public async Task Handle_InsufficientStock_ReturnsFailure()
    {
        var product = Product.Create("Widget", 9.99m, 2);
        _productRepository.GetByIdAsync(product.Id).Returns(product);

        var result = await _handler.Handle(new AddToCartCommand(Guid.NewGuid().ToString(), product.Id, 5), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Insufficient stock. Available: 2.", result.Error);
    }

    [Fact]
    public async Task Handle_ZeroQuantity_ReturnsFailure()
    {
        var result = await _handler.Handle(new AddToCartCommand(Guid.NewGuid().ToString(), Guid.NewGuid(), 0), CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal("Quantity must be greater than zero.", result.Error);
    }
}
