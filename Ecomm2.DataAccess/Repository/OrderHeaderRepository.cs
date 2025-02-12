using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;

namespace Ecomm2.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader> ,IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderHeaderRepository(ApplicationDbContext context) : base(context)
        {
            _context= context;
        }
                
    }
}
