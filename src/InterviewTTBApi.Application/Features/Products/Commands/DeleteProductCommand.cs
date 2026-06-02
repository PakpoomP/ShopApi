using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;

namespace InterviewTTBApi.Application.Features.Products.Commands;

public record DeleteProductCommand(Guid Id) : IRequest<Result>;

public class DeleteProductCommandHandler(IProductRepository repository)
    : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetByIdAsync(request.Id);
        if (product is null)
            return Result.Failure("Product not found.");

        await repository.DeleteAsync(request.Id);
        return Result.Success();
    }
}
