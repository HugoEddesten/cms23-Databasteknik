using DbAssignment.Contexts;
using DbAssignment.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbAssignment.Repositories;

public class CustomerRepository : Repository<CustomerEntity>
{
    private readonly DataContext _context;

    public CustomerRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<CustomerEntity>> GetAllAsync()
    {
        return await _context.Customers.Include(x => x.Address).Include(x => x.Information).ThenInclude(x => x.CustomerInformationTypesEntity).ToListAsync() ?? null!;
    }

    public override async Task<CustomerEntity> GetAsync(Expression<Func<CustomerEntity, bool>> expression)
    {
        return await _context.Customers.Include(x => x.Address).Include(x => x.Information).FirstOrDefaultAsync(expression) ?? null!;
    }
}
