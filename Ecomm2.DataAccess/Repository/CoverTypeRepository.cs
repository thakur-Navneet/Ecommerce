using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;

namespace Ecomm2.DataAccess.Repository
{
    public class CoverTypeRepository : Repository<CoverType>,ICoverTypeRepository
    {
        private readonly ApplicationDbContext _context;
        public CoverTypeRepository(ApplicationDbContext context) : base(context)
        {
            _context= context;
        }
    }
}
