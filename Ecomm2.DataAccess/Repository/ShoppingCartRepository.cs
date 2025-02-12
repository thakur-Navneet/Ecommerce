using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;

namespace Ecomm2.DataAccess.Repository
{
    public class ShoppingCartRepository : Repository<ShoppingCart> , IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context; 
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
