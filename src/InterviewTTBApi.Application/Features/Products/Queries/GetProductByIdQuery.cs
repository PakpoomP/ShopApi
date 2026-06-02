using InterviewTTBApi.Application.Features.Products.Dtos;
using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;

namespace InterviewTTBApi.Application.Features.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<Result<ProductDto>>;

public class GetProductByIdQueryHandler(IProductRepository repository)
    : IRequestHandler<GetProductByIdQuery, Result<ProductDto>>
{
    public async Task<Result<ProductDto>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id);
        if (product is null)
            return Result<ProductDto>.Failure("Product not found.");

        return Result<ProductDto>.Success(new ProductDto(product.Id, product.Name, product.Price, product.Stock));
    }
}
