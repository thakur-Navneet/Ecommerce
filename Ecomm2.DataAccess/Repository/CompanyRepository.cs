using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;

namespace Ecomm2.DataAccess.Repository
{
    public class CompanyRepositroy : Repository<Company>, ICompanyRepository
    {
        public readonly ApplicationDbContext _context;
        public CompanyRepositroy(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
