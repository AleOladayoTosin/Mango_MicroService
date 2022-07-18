using AutoMapper;
using Mango.Service.ProductAPI.DbContexts;
using Mango.Service.ProductAPI.Models;
using Mango.Service.ProductAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Service.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private IMapper _mapper;
        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto product)
        {
            Product productItem = _mapper.Map<ProductDto, Product>(product);
            if (productItem.ProductId > 0)
                _db.Products.Update(productItem);
            else
                _db.Products.Add(productItem);

            await _db.SaveChangesAsync();

            return _mapper.Map<Product, ProductDto>(productItem);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product productItem = await _db.Products.Where(c => c.ProductId == productId).FirstOrDefaultAsync();
                if (productItem == null)
                    return false;

                _db.Products.Remove(productItem);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProduct()
        {
            List<Product> productList = await _db.Products.ToListAsync();
            return _mapper.Map<List<ProductDto>>(productList);
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            Product productItem = await _db.Products.Where(c=>c.ProductId == productId).FirstOrDefaultAsync();
            return _mapper.Map<ProductDto>(productItem);
        }
    }
}
