using DbAssignment.Contexts;
using DbAssignment.Entities;
using DbAssignment.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Linq.Expressions;

namespace DbAssignment.Services;

public class AddressService
{
    private readonly DataContext _context;
    

    public AddressService(DataContext context)
    {
        _context = context;
    }

    public async Task<AddressEntity> CreateAddressAsync(AddressEntity address)
    {
        AddressEntity _address = await _context.Addresses.FirstOrDefaultAsync(x => x.StreetName == address.StreetName && x.StreetNumber == address.StreetNumber && x.PostalCode == address.PostalCode && x.City == address.City) ?? null!;
        if (_address == null)
        {
            await _context.Addresses.AddAsync(address); // Might change to Add without async
            await _context.SaveChangesAsync();
            return address;
        }
        return _address;
    }
    public async Task<AddressEntity> GetAddressAsync(Expression<Func<AddressEntity, bool>> expression)
    {
        AddressEntity address = await _context.Addresses.FirstOrDefaultAsync(expression) ?? null!;
        return address ?? null!;
    }

}
