using APP.Core.Entities;
using APP.Core.Enums;
using APP.Repository.IRepo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APP.Repository.EFRepo.Repositories
{
    public abstract class BaseRepository<TEntity, TId> : IBaseRepository<TEntity, TId>
        where TEntity : BaseEntity<TId>
        where TId : class
    {

        public DbContext Context;

        public BaseRepository(DbContext context)
        {
            Context = context;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            _ = Context.Set<TEntity>().AddAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entities)
        {
            _ = Context.Set<TEntity>().AddRangeAsync(entities);
            return entities;
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public async Task<TEntity> GetAsync(TId id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().Where(x => x.RecordStatus != RecordStatus.DELETED && x.RecordStatus != RecordStatus.ARCHIVE).ToListAsync();
        }

        public virtual async Task<List<TEntity>> GetAllActiveAsync()
        {
            return await Context.Set<TEntity>().Where(x => x.RecordStatus == RecordStatus.ACTIVE).ToListAsync();
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity obj)
        {
            Context.Entry(obj).State = EntityState.Modified;
            return obj;
        }

        public virtual async Task DeleteFinallyAsync(TEntity obj)
        {
            Context.Set<TEntity>().Remove(obj);
        }

        public virtual async Task<int> CountAsync()
        {
            return await Query().CountAsync();
        }

        public virtual IQueryable<TEntity> Query()
        {
            return Context.Set<TEntity>().Where(x => x.RecordStatus != RecordStatus.DELETED && x.RecordStatus != RecordStatus.ARCHIVE);
        }

        public virtual async Task<bool> ExistsAsync(TId id)
        {
            return await Context.Set<TEntity>().AnyAsync(e => e.Id.Equals(id));
        }
    }
}