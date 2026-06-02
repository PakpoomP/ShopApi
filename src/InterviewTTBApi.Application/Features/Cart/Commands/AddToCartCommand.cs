using InterviewTTBApi.Application.Interfaces;
using InterviewTTBApi.Domain.Common;
using InterviewTTBApi.Domain.Entities;
using InterviewTTBApi.Domain.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Application.Features.Cart.Commands;

public record AddToCartCommand(string CartId, Guid ProductId, int Quantity) : IRequest<Result>;

public class AddToCartCommandHandler(IApplicationDbContext context, IProductRepository productRepository)
    : IRequestHandler<AddToCartCommand, Result>
{
    public async Task<Result> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        if (request.Quantity <= 0)
            return Result.Failure("Quantity must be greater than zero.");

        var product = await productRepository.GetByIdAsync(request.ProductId);
        if (product is null)
            return Result.Failure("Product not found.");

        if (product.Stock < request.Quantity)
            return Result.Failure($"Insufficient stock. Available: {product.Stock}.");

        var existingItem = await context.CartItems
            .FirstOrDefaultAsync(c => c.ProductId == request.ProductId, cancellationToken);

        if (existingItem is null)
        {
            var item = CartItem.Create(request.CartId, request.ProductId, request.Quantity);
            context.CartItems.Add(item);
        }
        else
        {
            existingItem.AddQuantity(request.Quantity);
            context.CartItems.Update(existingItem);
        }

        product.DeductStock(request.Quantity);
        await productRepository.UpdateAsync(product);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
