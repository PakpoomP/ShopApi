using InterviewTTBApi.Domain.Entities;
using InterviewTTBApi.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InterviewTTBApi.Infrastructure.Persistence.Repositories;

public class ProductRepository(AppDbContext context) : IProductRepository
{
    public async Task<IEnumerable<Product>> GetAllAsync() =>
        await context.Products.ToListAsync();

    public async Task<Product?> GetByIdAsync(Guid id) =>
        await context.Products.FindAsync(id);

    public async Task AddAsync(Product product)
    {
        context.Products.Add(product);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Product product)
    {
        context.Products.Update(product);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is not null)
        {
            context.Products.Remove(product);
            await context.SaveChangesAsync();
        }
    }
}
