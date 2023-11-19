using System.ComponentModel.DataAnnotations;

namespace DbAssignment.Entities;

public class CustomerInformationTypesEntity
{
    [Key]
    public int Id { get; set; }
    public string InformationType { get; set; } = null!;
}
