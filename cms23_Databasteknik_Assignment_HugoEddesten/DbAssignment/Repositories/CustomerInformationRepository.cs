using DbAssignment.Contexts;
using DbAssignment.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbAssignment.Repositories;

public class CustomerInformationRepository : Repository<CustomerInformationsEntity>
{
    private readonly DataContext _context;

    public CustomerInformationRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<CustomerInformationsEntity>> GetAllAsync()
    {
        return await _context.CustomerInformations.Include(x => x.CustomerInformationTypesEntity).ToListAsync();
    }
}
