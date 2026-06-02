
using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Entities;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;

namespace InterviewTTBApi.Application.Features.Products.Commands;

public record CreateProductCommand(string Name, decimal Price, int Stock) : IRequest<Result<Guid>>;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result<Guid>.Failure("Name is required.");

        if (request.Price <= 0)
            return Result<Guid>.Failure("Price must be greater than zero.");

        if (request.Stock < 0)
            return Result<Guid>.Failure("Stock cannot be negative.");

        var product = Product.Create(request.Name, request.Price, request.Stock);
        await _repository.AddAsync(product);
        return Result<Guid>.Success(product.Id);
    }
}
