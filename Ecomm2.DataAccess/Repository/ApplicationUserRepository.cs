using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Ecomm2.Models;

namespace Ecomm2.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
