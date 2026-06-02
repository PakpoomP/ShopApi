using InterviewTTBApi.Application.Interfaces;
using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Application.Features.Cart.Commands;

public record UpdateCartItemCommand(string CartId, Guid ProductId, int Quantity) : IRequest<Result>;

public class UpdateCartItemCommandHandler(IApplicationDbContext db, IProductRepository productRepository)
    : IRequestHandler<UpdateCartItemCommand, Result>
{
    public async Task<Result> Handle(UpdateCartItemCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
            return Result.Failure("Quantity must be greater than zero.");

        var item = await db.CartItems
            .FirstOrDefaultAsync(c => c.CartId == request.CartId && c.ProductId == request.ProductId, cancellationToken);
        if (item is null)
            return Result.Failure("Cart item not found.");

        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            return Result.Failure("Product not found.");

        var delta = request.Quantity - item.Quantity;
        if (delta > 0 && product.Stock < delta)
            return Result.Failure($"Insufficient stock. Available: {product.Stock}.");

        if (delta > 0)
            product.DeductStock(delta);
        else if (delta < 0)
            product.RestoreStock(-delta);

        item.SetQuantity(request.Quantity);
        db.CartItems.Update(item);
        await productRepository.UpdateAsync(product);
        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
