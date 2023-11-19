using System.ComponentModel.DataAnnotations;

namespace DbAssignment.Entities;

public class CustomerEntity
{
    [Key] public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set;} = null!;
    public int AddressId { get; set; }
    public AddressEntity Address { get; set; } = new AddressEntity();
    public IEnumerable<CustomerInformationsEntity> Information { get; set; } = new List<CustomerInformationsEntity>();

}
