using InterviewTTBApi.Application.Features.Products.Queries;
using InterviewTTBApi.Domain.Entities;
using InterviewTTBApi.Domain.Interfaces;
using NSubstitute;

namespace InterviewTTBApi.Application.UnitTests.Features.Products;

public class GetProductsQueryHandlerTests
{
    private readonly IProductRepository _repository = Substitute.For<IProductRepository>();
    private readonly GetProductsQueryHandler _handler;

    public GetProductsQueryHandlerTests()
    {
        _handler = new GetProductsQueryHandler(_repository);
    }

    [Fact]
    public async Task Handle_ReturnsAllProductsAsDtos()
    {
        var products = new List<Product>
        {
            Product.Create("Widget", 9.99m, 50),
            Product.Create("Gadget", 29.99m, 10)
        };
        _repository.GetAllAsync().Returns(products);

        var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        var dtos = result.Value!.ToList();
        Assert.Equal(2, dtos.Count);
        Assert.Equal("Widget", dtos[0].Name);
        Assert.Equal(9.99m, dtos[0].Price);
        Assert.Equal(50, dtos[0].Stock);
    }

    [Fact]
    public async Task Handle_EmptyRepository_ReturnsEmptyList()
    {
        _repository.GetAllAsync().Returns(new List<Product>());

        var result = await _handler.Handle(new GetProductsQuery(), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }
}
