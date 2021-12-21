using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarrantyRegistrationApp.Models;

namespace WarrantyRegistrationApp.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal WarrantyDataContext _context;
        internal DbSet<TEntity> _dbSet;


        public Repository(WarrantyDataContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = _dbSet;

            return query.ToList();
        }

        public async Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync()
        {
            IQueryable<TEntity> query = _dbSet;

            return await query.ToListAsync();
        }

        public virtual TEntity GetByID(int? id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual void Delete(int? id)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete);
            _context.SaveChanges();
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
