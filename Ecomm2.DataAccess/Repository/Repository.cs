
using Ecomm2.DataAccess.Data;
using Ecomm2.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Ecomm2.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
            dbSet = _context.Set<T>();
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;  // Start with the entity set.

            if (filter != null)  // If a filter is provided, apply it to the query.
                query = query.Where(filter);

            if (includeProperties != null)  // If related entities are to be included.
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);  // Use Include to load related entities.
                }
            }
            return query.FirstOrDefault();  // Return the first entity or null if none matches.
        }

        public T Get(int id)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = null)
        {
            IQueryable<T> query = dbSet;  // Start with the entity set.

            if (filter != null)  // Apply filtering if a filter is provided.
                query = query.Where(filter);

            if (includeProperties != null)  // If related entities should be loaded.
            {
                foreach (var includeProp in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);  // Load related entities.
                }
            }

            if (orderBy != null)  // If an ordering function is provided, apply it.
                return orderBy(query).ToList();

            return query.ToList();  // Return the list of entities.
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void Remove(int id)
        {
            dbSet.Remove(Get(id));
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.ChangeTracker.Clear(); // it is used for images
            dbSet.Update(entity);  // Mark the entity as modified for update.
        }
    }
}
