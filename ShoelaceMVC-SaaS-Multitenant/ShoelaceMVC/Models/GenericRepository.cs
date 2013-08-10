using System.Linq.Expressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoelaceMVC.Models;

namespace ShoelaceMVC.Repositories
{
    public class GenericRepository<TEntity> where TEntity : BaseEntity
    {
        internal ShoelaceDbContext context;
        internal DbSet<TEntity> dbSet;
        internal int tenantId;

        public GenericRepository(ShoelaceDbContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public GenericRepository(ShoelaceDbContext context, int tenantId)
        {
            this.tenantId = tenantId;
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            query = query.Where(x => x.AccountId == tenantId);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetByID(object id)
        {
            return dbSet.Where(x => x.AccountId == tenantId).First();
        }

        public virtual void Insert(TEntity entity)
        {
            entity.AccountId = tenantId;
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            if (entityToDelete.AccountId == tenantId)
            {
                Delete(entityToDelete); 
            }
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (entityToDelete.AccountId == tenantId)
            {
                if (context.Entry(entityToDelete).State == EntityState.Detached)
                {
                    dbSet.Attach(entityToDelete);
                }
                dbSet.Remove(entityToDelete);
            }
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            if (entityToUpdate.AccountId == tenantId)
            {
                dbSet.Attach(entityToUpdate);
                context.Entry(entityToUpdate).State = EntityState.Modified;
            }
        }
    }
}
