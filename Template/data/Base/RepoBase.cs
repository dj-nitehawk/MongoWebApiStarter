using MongoDB.Entities;
using MongoDB.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoWebApiStarter.Data.Base
{
    /// <summary>
    /// Base class for repos.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for the repo</typeparam>
    public class RepoBase<TEntity> where TEntity : Entity
    {
        //FIND

        /// <summary>
        /// Find an entity by ID
        /// </summary>
        /// <param name="id">The ID to search by</param>
        /// <returns></returns>
        public TEntity Find(string id)
        {
            return FindAsync<TEntity>(e => e.ID == id, null).GetAwaiter().GetResult().SingleOrDefault();
        }

        /// <summary>
        /// Find an entity by ID
        /// </summary>
        /// <param name="id">The ID to search by</param>
        /// <returns></returns>
        public async Task<TEntity> FindAsync(string id)
        {
            return (await FindAsync<TEntity>(e => e.ID == id, null)).SingleOrDefault();
        }

        /// <summary>
        /// Find an entity by ID with a projection
        /// </summary>
        /// <typeparam name="TResult">The projected entity type</typeparam>
        /// <param name="id">The ID to search by</param>
        /// <param name="projection">A projection expression</param>
        /// <returns></returns>
        public TResult Find<TResult>(string id, Expression<Func<TEntity, TResult>> projection)
        {
            return FindAsync(e => e.ID == id, projection).GetAwaiter().GetResult().SingleOrDefault();
        }

        /// <summary>
        /// Find an entity by ID with a projection
        /// </summary>
        /// <typeparam name="TResult">The projected entity type</typeparam>
        /// <param name="id">The ID to search by</param>
        /// <param name="projection">A projection expression</param>
        /// <returns></returns>
        public async Task<TResult> FindAsync<TResult>(string id, Expression<Func<TEntity, TResult>> projection)
        {
            return (await FindAsync(e => e.ID == id, projection)).SingleOrDefault();
        }

        /// <summary>
        /// Find entities with a search criteria
        /// </summary>
        /// <param name="condition">An expression specifiying the search criteria</param>
        public List<TEntity> Find(Expression<Func<TEntity, bool>> condition)
        {
            return FindAsync<TEntity>(condition, null).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find entities with a search criteria
        /// </summary>
        /// <param name="condition">An expression specifiying the search criteria</param>
        public async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await FindAsync<TEntity>(condition, null);
        }

        /// <summary>
        /// Find entities with a search criteria with an optional projection
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="condition">An expression specifiying the search criteria</param>
        /// <param name="projection">A projection expression</param>
        /// <returns></returns>
        public List<TResult> Find<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> projection = null)
        {
            return FindAsync(condition, projection).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find entities with a search criteria with an optional projection
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="condition">An expression specifiying the search criteria</param>
        /// <param name="projection">A projection expression</param>
        /// <returns></returns>
        public async Task<List<TResult>> FindAsync<TResult>(Expression<Func<TEntity, bool>> condition, Expression<Func<TEntity, TResult>> projection = null)
        {
            var cmd = DB.Find<TEntity, TResult>().Match(condition);
            if (projection != null) cmd = cmd.Project(projection);
            return await cmd.ExecuteAsync();
        }

        //SAVE

        /// <summary>
        /// Persists an entity to the database replacing the existing data.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns>The ID of the saved entity</returns>
        public string Save(TEntity entity)
        {
            entity.Save();
            return entity.ID;
        }

        /// <summary>
        /// Persists an entity to the database replacing the existing data.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns>The ID of the saved entity</returns>
        public async Task<string> SaveAsync(TEntity entity)
        {
            await entity.SaveAsync();
            return entity.ID;
        }

        //DELETE

        /// <summary>
        /// Delete an entity by ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        public void Delete(string id)
        {
            DB.Delete<TEntity>(id);
        }

        /// <summary>
        /// Delete an entity by ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        public async Task DeleteAsync(string id)
        {
            await DB.DeleteAsync<TEntity>(id);
        }


        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public void Delete(TEntity entity)
        {
            entity.Delete();
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public async Task DeleteAsync(TEntity entity)
        {
            await entity.DeleteAsync();
        }
    }
}
