using System;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IRepository<T>
    {
        T Get(System.Guid id);
        IQueryable<T> GetAll();
        Task<T> SaveAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}