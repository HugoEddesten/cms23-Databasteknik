using DbAssignment.Contexts;
using DbAssignment.Entities;

namespace DbAssignment.Repositories;

public class CustomerInformationTypeRepository : Repository<CustomerInformationTypesEntity>
{
    private readonly DataContext _context;

    public CustomerInformationTypeRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}
