using DbAssignment.Contexts;
using DbAssignment.Entities;
using DbAssignment.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Linq.Expressions;

namespace DbAssignment.Services;

public class CustomerInformationsService
{
    private readonly CustomerInformationRepository _customerInformationRepository;

    public CustomerInformationsService(CustomerInformationRepository customerInformationRepository)
    {
        _customerInformationRepository = customerInformationRepository;
    }

    public async Task<IEnumerable<CustomerInformationsEntity>> GetInformationAsync(CustomerEntity customer)
    {
        List<CustomerInformationsEntity> customerInformations = new List<CustomerInformationsEntity>();
        IEnumerable<CustomerInformationsEntity> AllInformationsEntities = await _customerInformationRepository.GetAllAsync();
        foreach (var entity in AllInformationsEntities)
        {
            if (entity.Customer == customer)
            {
                customerInformations.Add(entity);
            }
        }
        return customerInformations;
    }
}
