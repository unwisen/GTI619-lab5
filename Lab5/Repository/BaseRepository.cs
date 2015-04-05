using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lab5.Models;

namespace Lab5.Repository
{
    public class BaseRepository<TObject> where TObject : class
    {
        protected ApplicationDbContext context;

        public BaseRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public ICollection<TObject> GetAll()
        {
            return context.Set<TObject>().ToList();
        }

        public async Task<ICollection<TObject>> GetAllAsync()
        {
            return await context.Set<TObject>().ToListAsync();
        }

        public TObject Get(int id)
        {
            return context.Set<TObject>().Find(id);
        }

        public async Task<TObject> GetAsync(int id)
        {
            return await context.Set<TObject>().FindAsync(id);
        }

        public TObject Find(Expression<Func<TObject, bool>> match)
        {
            return context.Set<TObject>().SingleOrDefault(match);
        }

        public async Task<TObject> FindAsync(Expression<Func<TObject, bool>> match)
        {
            return await context.Set<TObject>().SingleOrDefaultAsync(match);
        }

        public ICollection<TObject> FindAll(Expression<Func<TObject, bool>> match)
        {
            return context.Set<TObject>().Where(match).ToList();
        }

        public async Task<ICollection<TObject>> FindAllAsync(Expression<Func<TObject, bool>> match)
        {
            return await context.Set<TObject>().Where(match).ToListAsync();
        }

        public TObject Add(TObject t)
        {
            context.Set<TObject>().Add(t);
            context.SaveChanges();
            return t;
        }

        public async Task<TObject> AddAsync(TObject t)
        {
            context.Set<TObject>().Add(t);
            await context.SaveChangesAsync();
            return t;
        }

        public TObject Update(TObject updated, int key)
        {
            if (updated == null)
                return null;

            TObject existing = context.Set<TObject>().Find(key);
            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(updated);
                context.SaveChanges();
            }
            return existing;
        }

        public async Task<TObject> UpdateAsync(TObject updated, int key)
        {
            if (updated == null)
                return null;

            TObject existing = await context.Set<TObject>().FindAsync(key);
            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(updated);
                await context.SaveChangesAsync();
            }
            return existing;
        }

        public void Delete(TObject t)
        {
            context.Set<TObject>().Remove(t);
            context.SaveChanges();
        }

        public async Task<int> DeleteAsync(TObject t)
        {
            context.Set<TObject>().Remove(t);
            return await context.SaveChangesAsync();
        }

        public int Count()
        {
            return context.Set<TObject>().Count();
        }

        public async Task<int> CountAsync()
        {
            return await context.Set<TObject>().CountAsync();
        }
    }
}