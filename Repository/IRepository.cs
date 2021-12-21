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
        void Insert(TEntity entity);
        void Update(TEntity entityToUpdate);
        void Dispose();
    }
}
