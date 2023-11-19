using DbAssignment.Contexts;
using DbAssignment.Entities;
using DbAssignment.Models;
using DbAssignment.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace DbAssignment.Services;

public class CustomerService
{
    private readonly AddressRepository _addressRepository;
    private readonly CustomerInformationRepository _customerInformationRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly CustomerInformationTypeRepository _customerInformationTypeRepository;

    public CustomerService(AddressRepository addressRepository, CustomerInformationRepository customerInformationRepository, CustomerRepository customerRepository, CustomerInformationTypeRepository customerInformationTypeRepository)
    {
        _addressRepository = addressRepository;
        _customerInformationRepository = customerInformationRepository;
        _customerRepository = customerRepository;
        _customerInformationTypeRepository = customerInformationTypeRepository;
    }

    public async Task CreateCustomerAsync(CustomerRegistration _customer)
    {
        if (!await _customerInformationRepository.ExistsAsync(x => x.Value == _customer.Email))
        {
            AddressEntity address = new AddressEntity
            {
                StreetName = _customer.StreetName,
                StreetNumber = _customer.StreetNumber,
                PostalCode = _customer.PostalCode,
                City = _customer.City,
            };
            await _addressRepository.CreateAsync(address);
            
            CustomerEntity customer = new CustomerEntity
            {
                FirstName = _customer.FirstName,
                LastName = _customer.LastName,
                Address = address,
            };
            await _customerRepository.CreateAsync(customer);

            CustomerInformationsEntity customerEmail = new CustomerInformationsEntity
            {
                Customer = customer,
                CustomerInformationTypesEntity = await _customerInformationTypeRepository.GetAsync(x => x.InformationType == "Email"),
                Value = _customer.Email,
            };
           
            await _customerInformationRepository.CreateAsync(customerEmail);

            CustomerInformationsEntity customerPhone = new CustomerInformationsEntity
            {
                Customer = customer,
                CustomerInformationTypesEntity = await _customerInformationTypeRepository.GetAsync(x => x.InformationType == "Phone"),
                Value = _customer.Phone,
            };
            await _customerInformationRepository.CreateAsync(customerPhone);

        }
    }
    public async Task<IEnumerable<CustomerEntity>> GetAllCustomersAsync()
    {
        return await _customerRepository.GetAllAsync();
    }

    public async Task UpdateCustomerAsync(CustomerEntity customer)
    {
        await _customerRepository.UpdateAsync(customer);
    }

    public async Task<bool> DeleteCustomerAsync(CustomerEntity customer)
    {
        try
        {
            var emailInformationEntity = customer.Information.FirstOrDefault(x => x.CustomerInformationTypesEntity.InformationType == "Email");
            await _customerRepository.DeleteAsync(customer);

            var customerWithSameAddress = await _customerRepository.GetAsync(x => x.Address == customer.Address);
            if (customerWithSameAddress == null)
            {
                await _addressRepository.DeleteAsync(customer.Address);
            }

        } catch (Exception ex) { Debug.WriteLine(ex); }
        return false;


    }
}
