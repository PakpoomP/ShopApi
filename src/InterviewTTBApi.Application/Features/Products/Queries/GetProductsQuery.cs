
using InterviewTTBApi.Application.Features.Products.Dtos;
using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;
namespace InterviewTTBApi.Application.Features.Products.Queries;

public record GetProductsQuery : IRequest<Result<IEnumerable<ProductDto>>>;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, Result<IEnumerable<ProductDto>>>
{
    private readonly IProductRepository _repository;

    public GetProductsQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<IEnumerable<ProductDto>>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _repository.GetAllAsync();
        var dtos = products.Select(p => new ProductDto(p.Id, p.Name, p.Price, p.Stock));
        return Result<IEnumerable<ProductDto>>.Success(dtos);
    }
}
