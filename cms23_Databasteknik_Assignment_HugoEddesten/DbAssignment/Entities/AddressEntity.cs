using System.ComponentModel.DataAnnotations;

namespace DbAssignment.Entities;

public class AddressEntity
{
    [Key] public int Id { get; set; }
    public string StreetName { get; set; } = null!;
    public string? StreetNumber { get; set; }
    public string PostalCode { get; set; } = null!;
    public string City { get; set; } = null!;
}
