using Microsoft.EntityFrameworkCore;
using BiblioManager.DataAccess.Context;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;

namespace BiblioManager.DataAccess.Repositories;

public class BookRepository : GenericRepository<Book>, IBookRepository
{
    public BookRepository(LibraryDbContext context) : base(context)
    {
    }
    public async Task<Book?> GetByIsbnAsync(string isbn)
    {
        return await _dbSet
            .FirstOrDefaultAsync(b =>
                b.Isbn.ToLower() == isbn.ToLower());
    }
}