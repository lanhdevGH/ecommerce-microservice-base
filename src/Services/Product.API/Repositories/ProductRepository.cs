using Contracts.Common.Interfaces;
using Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistences;
using Product.API.Repositories.Interfaces;

namespace Product.API.Repositories
{
    public class ProductRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork) : RepositoryBase<CatalogProduct, long, ProductContext>(dbContext, unitOfWork), IProductRepository
    {
        public async Task<IEnumerable<CatalogProduct>> GetProducts() => await FindAll().ToListAsync();

        public Task<CatalogProduct?> GetProduct(long id) => GetByIdAsync(id);

        public Task<CatalogProduct?> GetProductByNo(string productNo) =>
           FindByCondition(x => x.No.Equals(productNo)).SingleOrDefaultAsync();

        public Task CreateProduct(CatalogProduct product) => CreateAsync(product);

        public Task UpdateProduct(CatalogProduct product) => UpdateAsync(product);

        public async Task DeleteProduct(long id)
        {
            var product = await GetProduct(id);
            if (product != null) await DeleteAsync(product); // Fix: Added 'await' to ensure the asynchronous operation is awaited.  
        }
    }
}
