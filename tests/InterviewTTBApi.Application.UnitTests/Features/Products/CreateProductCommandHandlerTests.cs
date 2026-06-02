using InterviewTTBApi.Application.Features.Products.Commands;
using InterviewTTBApi.Domain.Entities;
using InterviewTTBApi.Domain.Interfaces;
using NSubstitute;

namespace InterviewTTBApi.Application.UnitTests.Features.Products;

public class CreateProductCommandHandlerTests
{
    private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
    private readonly CreateProductCommandHandler _handler;

    public CreateProductCommandHandlerTests()
    {
        _handler = new CreateProductCommandHandler(_repository);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesProductAndReturnsId()
    {
        var command = new CreateProductCommand("Widget", 9.99m, 100);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value);
        await _repository.Received(1).AddAsync(Arg.Is<Product>(p =>
            p.Name == "Widget" && p.Price == 9.99m && p.Stock == 100));
    }

    [Theory]
    [InlineData("", 9.99, 10, "Name is required.")]
    [InlineData("  ", 9.99, 10, "Name is required.")]
    [InlineData("Widget", 0, 10, "Price must be greater than zero.")]
    [InlineData("Widget", -1, 10, "Price must be greater than zero.")]
    [InlineData("Widget", 9.99, -1, "Stock cannot be negative.")]
    public async Task Handle_InvalidCommand_ReturnsFailure(string name, decimal price, int stock, string expectedError)
    {
        var command = new CreateProductCommand(name, price, stock);

        var result = await _handler.Handle(command, CancellationToken.None);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedError, result.Error);
        await _repository.DidNotReceive().AddAsync(Arg.Any<Product>());
    }
}
