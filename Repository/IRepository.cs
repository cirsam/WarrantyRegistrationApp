using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WarrantyRegistrationApp.Repository
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Delete(int? id);
        void Delete(TEntity entityToDelete);
        IEnumerable<TEntity> GetAll();
        Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync();
        TEntity GetByID(int? id);
        Task<TEntity> GetByIDAsync(int? id);
        Task<IEnumerable<TEntity>> GetBySerialNumberAsync(string serialNumber);
        Task<bool> IsExistsAsync(int? id);
        void Insert(TEntity entity);
        Task InsertAsync(TEntity entity);
        void Update(TEntity entityToUpdate);
        Task UpdateAsync(TEntity entityToUpdate);
        void Dispose();
    }
}
