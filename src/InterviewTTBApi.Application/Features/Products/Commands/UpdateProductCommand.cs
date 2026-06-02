using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;

namespace InterviewTTBApi.Application.Features.Products.Commands;

public record UpdateProductCommand(Guid Id, string Name, decimal Price, int Stock) : IRequest<Result>;

public class UpdateProductCommandHandler(IProductRepository repository)
    : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
            return Result.Failure("Name is required.");

        if (request.Price <= 0)
            return Result.Failure("Price must be greater than zero.");

        if (request.Stock < 0)
            return Result.Failure("Stock cannot be negative.");

        var product = await repository.GetByIdAsync(request.Id);
        if (product is null)
            return Result.Failure("Product not found.");

        product.Update(request.Name, request.Price, request.Stock);
        await repository.UpdateAsync(product);
        return Result.Success();
    }
}
