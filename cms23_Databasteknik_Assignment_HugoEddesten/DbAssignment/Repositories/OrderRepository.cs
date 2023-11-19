using DbAssignment.Contexts;
using DbAssignment.Entities;

namespace DbAssignment.Repositories;

public class OrderRepository : Repository<OrderEntity>
{
    private readonly DataContext _context;

    public OrderRepository(DataContext context) : base(context)
    {
        _context = context;
    }
}
