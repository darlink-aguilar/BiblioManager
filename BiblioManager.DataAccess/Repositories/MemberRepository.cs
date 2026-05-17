using Microsoft.EntityFrameworkCore;
using BiblioManager.DataAccess.Context;
using BiblioManager.Domain.Entities;
using BiblioManager.Domain.Interfaces.Repositories;

namespace BiblioManager.DataAccess.Repositories;

public class MemberRepository : GenericRepository<Member>, IMemberRepository
{
    public MemberRepository(LibraryDbContext context) : base(context)
    {
    }
    public async Task<Member?> GetByDniAsync(string dni)
    {
        return await _dbSet
            .FirstOrDefaultAsync(m =>
                m.Dni.ToLower() == dni.ToLower());
    }
}