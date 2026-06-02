using InterviewTTBApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Product> Products { get; }
    DbSet<CartItem> CartItems { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
