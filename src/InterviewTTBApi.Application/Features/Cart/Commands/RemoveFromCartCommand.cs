using InterviewTTBApi.Application.Interfaces;
using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Application.Features.Cart.Commands;

public record RemoveFromCartCommand(string CartId, Guid ProductId) : IRequest<Result>;

public class RemoveFromCartCommandHandler(IApplicationDbContext db, IProductRepository productRepository)
    : IRequestHandler<RemoveFromCartCommand, Result>
{
    public async Task<Result> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var item = await db.CartItems
            .FirstOrDefaultAsync(c => c.ProductId == request.ProductId, cancellationToken);
        if (item is null)
            return Result.Failure("Cart item not found.");

        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is not null)
            product.RestoreStock(item.Quantity);

        db.CartItems.Remove(item);

        if (product is not null)
            await productRepository.UpdateAsync(product);

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
