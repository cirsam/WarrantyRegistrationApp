using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

        public ActionResult<IEnumerable<TEntity>> GetAllByCustomerAsync(string customerID)
        {
            //IQueryable<TEntity> query = _dbSet;
            IList<TEntity> entities = new List<TEntity>();
            foreach (var entity in _dbSet)
            {
                var entityCustomerId = entity.GetType().GetProperty("userId").GetValue(entity);
                if (entityCustomerId.ToString() == customerID)
                {
                    entities.Add(entity);
                }
            }

            return entities.ToList();
        }

        public IEnumerable<TEntity> GetAllByCustomer(string customerID)
        {
            IList<TEntity> entities = new List<TEntity>();
            foreach (var entity in _dbSet)
            {
                var entityCustomerId = entity.GetType().GetProperty("userId").GetValue(entity);
                if (entityCustomerId?.ToString() == customerID)
                {
                    entities.Add(entity);
                }
            }

            return entities.ToList();
        }

        public virtual TEntity GetByID(int? id)
        {
            return _dbSet.Find(id);
        }

        public virtual async Task<TEntity> GetByIDAsync(int? id)
        {
            return await _dbSet.FindAsync(id);
        }

        public TEntity GetByID(string userId)
        {
            var entitySerialNumber = _dbSet.Where(a => a.GetType().GetProperty("userId").Equals(userId));

            return _dbSet.Find(1);
        }


        public async Task<IEnumerable<TEntity>> GetBySerialNumberAsync(string serialNumber)
        {
            IList<TEntity> entities = new List<TEntity>();
            foreach (var entity in _dbSet)
            {
                var entitySerialNumber = entity.GetType().GetProperty("ProductSerialNumber").GetValue(entity);
                if (entitySerialNumber.ToString() == serialNumber)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public IEnumerable<TEntity> GetBySerialNumber(string serialNumber)
        {
            IList<TEntity> entities = new List<TEntity>();
            foreach (var entity in _dbSet)
            {
                var entitySerialNumber = entity.GetType().GetProperty("ProductSerialNumber").GetValue(entity);
                if (entitySerialNumber.ToString()==serialNumber)
                {
                    entities.Add(entity);
                }
            }

            return entities;
        }

        public async Task<bool> IsExistsAsync(int? id)
        {
            var entiy = await _dbSet.FindAsync(id);

            if (entiy.Equals(null))
            {
                return true;
            }

            return false;
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            _dbSet.Add(entity);
            await _context.SaveChangesAsync();
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

        public virtual async Task UpdateAsync(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
