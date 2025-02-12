using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;

namespace Ecomm2.DataAccess.Repository
{
    public class ProductRepositrory : Repository<Product> , IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepositrory(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
