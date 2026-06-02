using InterviewTTBApi.Application.Interfaces;
using InterviewTTBApi.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Application.Features.Cart.Commands;

public record CheckoutCartCommand(string CartId) : IRequest<Result>;

public class CheckoutCartCommandHandler(IApplicationDbContext db)
    : IRequestHandler<CheckoutCartCommand, Result>
{
    public async Task<Result> Handle(CheckoutCartCommand request, CancellationToken cancellationToken)
    {
        var items = await db.CartItems
            // .Where(c => c.CartId == request.CartId)
            .ToListAsync(cancellationToken);

        if (items.Count == 0)
            return Result.Failure("Cart not found.");

        foreach (var item in items)
            item.Checkout();

        await db.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
