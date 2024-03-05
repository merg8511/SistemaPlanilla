using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SistemaOrbita.DAL.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> Get(string id);

        Task<IEnumerable<T>> GetAll(
            Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeProperties = null,
            bool isTracking = true
            );

        Task<T> GetFirst(
            Expression<Func<T, bool>> filter = null,
            string includeProperties = null,
            bool isTracking = true
            );

        Task Create(T entity);

        void Delete(T entity);

        void DeleteRange(IEnumerable<T> entities);

    }
}
