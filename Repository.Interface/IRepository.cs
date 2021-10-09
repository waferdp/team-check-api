using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;

namespace Repository.Interface
{
    public interface IRepository<T> where T : Entity
    {
        T Get(System.Guid id);
        Task<IQueryable<T>> GetAllAsync();
        Task<T> SaveAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}