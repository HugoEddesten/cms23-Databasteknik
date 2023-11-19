using System.ComponentModel.DataAnnotations;

namespace DbAssignment.Entities;

public class OrderEntity
{
    [Key]
    public int Id { get; set; }
    public CustomerEntity Customer { get; set; } = null!;
    public ICollection<ProductEntity> Orders { get; set; } = new HashSet<ProductEntity>();
}
