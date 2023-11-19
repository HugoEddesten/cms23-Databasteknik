using DbAssignment.Entities;
using DbAssignment.Repositories;
using System.Diagnostics;

namespace DbAssignment.Services;

public class ProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task CreateProductAsync(ProductEntity product)
    {
        await _productRepository.CreateAsync(product);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            ProductEntity product = await _productRepository.GetAsync(x => x.Id == id);
            return await _productRepository.DeleteAsync(product);
        } catch (Exception ex) { Debug.WriteLine(ex); }
        return false;
    }

    public async Task<IEnumerable<ProductEntity>> GetAllProducts() 
    {
        return await _productRepository.GetAllAsync();
    }
}
