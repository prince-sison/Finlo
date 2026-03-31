using Finlo.Application.Interfaces.Categories;
using Finlo.Domain.Entities;
using Finlo.Infrastructure.Data;

namespace Finlo.Infrastructure.Repositories.Categories;

public class CategoryRepository : BaseRepository<Category, Guid>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context) : base(context)
    {
    }
}