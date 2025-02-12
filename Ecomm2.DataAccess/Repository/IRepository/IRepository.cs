using System.Linq.Expressions;

namespace Ecomm2.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);
        void Remove (int id);
        void RemoveRange(IEnumerable<T> entities);
        T Get(int id);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,  // Optional filter expression (e.g., WHERE clause).
                              Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,  // Optional ordering logic (e.g., ORDER BY).
                              string includeProperties = null   // Optional related entities to include (e.g., for eager loading).
        );
        T FirstOrDefault(
                Expression<Func<T, bool>> filter = null,  // Optional filter expression (e.g., WHERE clause).
                string includeProperties = null  // Optional related entities to include (e.g., for eager loading).
        );
    }
}
