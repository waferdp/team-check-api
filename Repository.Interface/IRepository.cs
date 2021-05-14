using System;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;

namespace Repository.Interface
{
    public interface IRepository<T> where T : Entity
    {
        T Get(System.Guid id);
        IQueryable<T> GetAll();
        Task<T> SaveAsync(T entity);
        Task DeleteAsync(Guid id);
    }
}