using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DbAssignment.Entities;


public class CustomerInformationsEntity
{
    [Key] public int Id { get; set; }
    public CustomerEntity Customer { get; set; } = null!;
    public CustomerInformationTypesEntity CustomerInformationTypesEntity { get; set; } = null!;
    public string Value { get; set; } = null!;
}
