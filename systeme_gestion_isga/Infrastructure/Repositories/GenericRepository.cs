using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using systeme_gestion_isga.Infrastructure.Database;

namespace system_gestion_isga.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected AppDbContext _context;
        protected DbSet<T> _dbSet;

        public GenericRepository()
        {
            _context = new AppDbContext();
            _dbSet = _context.Set<T>();
        }

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public void Delete(object id)
        {
            T element = _dbSet.Find(id);
            if (element != null)
            {
                _dbSet.Remove(element);
            }
        }

        public IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        public T GetById(object id)
        {
            return _dbSet.Find(id);
        }

        public void Insert(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }
    }
}