using DbAssignment.Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAssignment.Contexts;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<CustomerInformationsEntity> CustomerInformations { get; set; }
    public DbSet<CustomerInformationTypesEntity> CustomerInformationTypes { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }
}

