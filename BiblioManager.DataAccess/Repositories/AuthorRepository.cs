using Microsoft.EntityFrameworkCore;
using BiblioManager.DataAccess.Context;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;

namespace BiblioManager.DataAccess.Repositories;

public class AuthorRepository : GenericRepository<Author>, IAuthorRepository
{
    public AuthorRepository(LibraryDbContext context) : base(context)
    {
    }
    public async Task<Author?> GetByNameAndLastNameAsync(string firstName, string lastName)
    {
        return await _dbSet
            .FirstOrDefaultAsync(a =>
                a.FirstName.ToLower() == firstName.ToLower() &&
                a.LastName.ToLower() == lastName.ToLower());
    }
}