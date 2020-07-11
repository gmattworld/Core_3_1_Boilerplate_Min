using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace APP.Repository.IRepo
{
    public interface IBaseRepository<TEntity, TId>
    {
        /// <summary>
        /// Create new record
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> InsertAsync(TEntity entity);

        /// <summary>
        /// Create multiple new record
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> InsertRangeAsync(IEnumerable<TEntity> entities);

        /// <summary>
        /// Find get records using predicates
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get record by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync(TId id);

        /// <summary>
        /// Get all records
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task<TEntity> UpdateAsync(TEntity obj);

        /// <summary>
        /// Get all active records
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetAllActiveAsync();

        /// <summary>
        /// Delete Record
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        Task DeleteFinallyAsync(TEntity obj);

        /// <summary>
        /// Count records
        /// </summary>
        /// <returns></returns>
        Task<int> CountAsync();

        /// <summary>
        /// Get queriable response
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Query();

        /// <summary>
        /// Check if exist
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> ExistsAsync(TId id);
    }
}