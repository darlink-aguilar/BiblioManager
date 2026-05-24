using Microsoft.EntityFrameworkCore;
using BiblioManager.DataAccess.Context;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;

namespace BiblioManager.DataAccess.Repositories
{
    public class BookAuthorRepository : GenericRepository<BookAuthor>, IBookAuthorRepository
    {
        public BookAuthorRepository(LibraryDbContext context) : base(context)
        {
        }

        public async Task<BookAuthor?> GetByBookAndAuthorAsync(int bookId, int authorId)
        {
            return await _dbSet
                .Where(ba => ba.BookId == bookId && ba.AuthorId == authorId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<BookAuthor>> GetByBookAsync(int bookId)
        {
            return await _dbSet
                .Where(ba => ba.BookId == bookId)
                .Include(ba => ba.Author)
                .ToListAsync();
        }
    }
}