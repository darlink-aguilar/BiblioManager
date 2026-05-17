using Microsoft.EntityFrameworkCore;
using BiblioManager.DataAccess.Context;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;

namespace BiblioManager.DataAccess.Repositories;

public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
{
    public CategoryRepository(LibraryDbContext context) : base(context)
    {
    }
    public async Task<Category?> GetByNameAsync(string name)
    {
        return await _dbSet
            .FirstOrDefaultAsync(c =>
                c.Name.ToLower() == name.ToLower());
    }

    //public async Task<bool> HasBooksAsync(int categoryId)
    //{
    //    return await _dbSet
    //        .AnyAsync(c => c.Id == categoryId && c.Books.Any());
    //}
}