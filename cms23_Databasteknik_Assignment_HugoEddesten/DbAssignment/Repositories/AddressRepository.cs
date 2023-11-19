using DbAssignment.Contexts;
using DbAssignment.Entities;

namespace DbAssignment.Repositories;

public class AddressRepository : Repository<AddressEntity>
{
    private readonly DataContext _context;

    public AddressRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}
