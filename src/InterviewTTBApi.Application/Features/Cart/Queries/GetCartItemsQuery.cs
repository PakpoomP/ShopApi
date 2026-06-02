using InterviewTTBApi.Application.Features.Cart.Dtos;
using InterviewTTBApi.Application.Interfaces;
using InterviewTTBApi.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Application.Features.Cart.Queries;

public record GetCartItemsQuery() : IRequest<Result<IEnumerable<CartItemDto>>>;

public class GetCartItemsQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetCartItemsQuery, Result<IEnumerable<CartItemDto>>>
{
    public async Task<Result<IEnumerable<CartItemDto>>> Handle(GetCartItemsQuery request, CancellationToken cancellationToken)
    {
        var items = await db.CartItems
            .Where(c => c.Status == false)
            .ToListAsync(cancellationToken);

        var dtos = items.Select(i => new CartItemDto(i.Id, i.CartId, i.ProductId, i.Quantity));
        return Result<IEnumerable<CartItemDto>>.Success(dtos);
    }
}
