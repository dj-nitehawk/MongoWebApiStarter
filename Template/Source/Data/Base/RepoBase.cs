using MongoDB.Entities;
using MongoDB.Entities.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// Base class for repos.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity for the repo</typeparam>
    public abstract class RepoBase<TEntity> where TEntity : IEntity
    {
        //FIND

        /// <summary>
        /// Find an entity by ID
        /// </summary>
        /// <param name="id">The ID to search by</param>
        public static TEntity Find(string id)
        {
            return FindAsync<TEntity>(e => e.ID == id, null).GetAwaiter().GetResult().SingleOrDefault();
        }

        /// <summary>
        /// Find an entity by ID
        /// </summary>
        /// <param name="id">The ID to search by</param>
        public static async Task<TEntity> FindAsync(string id)
        {
            return (await FindAsync<TEntity>(e => e.ID == id, null)).SingleOrDefault();
        }

        /// <summary>
        /// Find an entity by ID with a projection
        /// </summary>
        /// <typeparam name="TResult">The projected entity type</typeparam>
        /// <param name="id">The ID to search by</param>
        /// <param name="projection">A projection expression</param>
        public static TResult Find<TResult>(string id, Expression<Func<TEntity, TResult>> projection)
        {
            return FindAsync(e => e.ID == id, projection).GetAwaiter().GetResult().SingleOrDefault();
        }

        /// <summary>
        /// Find an entity by ID with a projection
        /// </summary>
        /// <typeparam name="TResult">The projected entity type</typeparam>
        /// <param name="id">The ID to search by</param>
        /// <param name="projection">A projection expression</param>
        public static async Task<TResult> FindAsync<TResult>(string id, Expression<Func<TEntity, TResult>> projection)
        {
            return (await FindAsync(e => e.ID == id, projection)).SingleOrDefault();
        }

        /// <summary>
        /// Find entities with a search criteria
        /// </summary>
        /// <param name="condition">An expression specifiying the search criteria</param>
        public static List<TEntity> Find(Expression<Func<TEntity, bool>> condition)
        {
            return FindAsync<TEntity>(condition).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find entities with a search criteria
        /// </summary>
        /// <param name="condition">An expression specifiying the search criteria</param>
        public static async Task<List<TEntity>> FindAsync(Expression<Func<TEntity, bool>> condition)
        {
            return await FindAsync<TEntity>(condition);
        }

        /// <summary>
        /// Find entities with a search expression and optional projecting, sorting and paging
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="condition">An expression specifiying the search criteria</param>
        /// <param name="projection">An optional projection expression</param>
        /// <param name="sortBy">x => x.FirstName (optional)</param>
        /// <param name="sortOrder">The sorting order (optional)</param>
        /// <param name="skip">The number of entities to skip (optional)</param>
        /// <param name="take">The number of entities to take (optional)</param>
        public static List<TResult> Find<TResult>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TResult>> projection = null,
            Expression<Func<TEntity, object>> sortBy = null,
            Sort sortOrder = Sort.None,
            int skip = 0,
            int take = 0)
        {
            return FindAsync(condition, projection, sortBy, sortOrder, skip, take).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find entities with a search expression and optional projecting, sorting and paging
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="condition">An expression specifiying the search criteria</param>
        /// <param name="projection">An optional projection expression</param>
        /// <param name="sortBy">x => x.FirstName (optional)</param>
        /// <param name="sortOrder">The sorting order (optional)</param>
        /// <param name="skip">The number of entities to skip (optional)</param>
        /// <param name="take">The number of entities to take (optional)</param>
        public static async Task<List<TResult>> FindAsync<TResult>(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, TResult>> projection = null,
            Expression<Func<TEntity, object>> sortBy = null,
            Sort sortOrder = Sort.None,
            int skip = 0,
            int take = 0
            )
        {
            var cmd = DB.Find<TEntity, TResult>().Match(condition);

            if (projection != null)
                cmd = cmd.Project(projection);

            if (sortOrder != Sort.None && sortBy != null)
                cmd = cmd.Sort(
                    sortBy,
                    sortOrder == Sort.Ascending
                    ? Order.Ascending
                    : Order.Descending);

            if (skip > 0)
                cmd.Skip(skip);

            if (take > 0)
                cmd.Limit(take);

            return await cmd.ExecuteAsync();
        }

        /// <summary>
        /// Find an entity by ID and an exclusion projection
        /// </summary>
        /// <param name="id">The ID to search by</param>
        /// <param name="exclusion">x => new { x.PropToExclude, x.AnotherPropEx }</param>
        public static TEntity FindExcluding(string id, Expression<Func<TEntity, object>> exclusion)
        {
            return FindExcludingAsync(e => e.ID == id, exclusion).GetAwaiter().GetResult().SingleOrDefault();
        }

        /// <summary>
        /// Find an entity by ID and an exclusion projection
        /// </summary>
        /// <param name="id">The ID to search by</param>
        /// <param name="exclusion">x => new { x.PropToExclude, x.AnotherPropEx }</param>
        public static async Task<TEntity> FindExcludingAsync(string id, Expression<Func<TEntity, object>> exclusion)
        {
            return (await FindExcludingAsync(e => e.ID == id, exclusion)).SingleOrDefault();
        }

        /// <summary>
        /// Find entities with a search expression and exclusion projecting, optional sorting and paging
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="condition">An expression specifiying the search criteria</param>
        /// <param name="exclusion">x => new { x.PropToExclude, x.AnotherPropEx }</param>
        /// <param name="sortBy">x => x.FirstName (optional)</param>
        /// <param name="sortOrder">The sorting order (optional)</param>
        /// <param name="skip">The number of entities to skip (optional)</param>
        /// <param name="take">The number of entities to take (optional)</param>
        public static List<TEntity> FindExcluding(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, object>> exclusion,
            Expression<Func<TEntity, object>> sortBy = null,
            Sort sortOrder = Sort.None,
            int skip = 0,
            int take = 0
            )
        {
            return FindExcludingAsync(condition, exclusion, sortBy, sortOrder, skip, take).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Find entities with a search expression and exclusion projecting, optional sorting and paging
        /// </summary>
        /// <typeparam name="TResult">The type of the result</typeparam>
        /// <param name="condition">An expression specifiying the search criteria</param>
        /// <param name="exclusion">x => new { x.PropToExclude, x.AnotherPropEx }</param>
        /// <param name="sortBy">x => x.FirstName (optional)</param>
        /// <param name="sortOrder">The sorting order (optional)</param>
        /// <param name="skip">The number of entities to skip (optional)</param>
        /// <param name="take">The number of entities to take (optional)</param>
        public static async Task<List<TEntity>> FindExcludingAsync(
            Expression<Func<TEntity, bool>> condition,
            Expression<Func<TEntity, object>> exclusion,
            Expression<Func<TEntity, object>> sortBy = null,
            Sort sortOrder = Sort.None,
            int skip = 0,
            int take = 0
            )
        {
            var cmd = DB.Find<TEntity>()
                        .Match(condition)
                        .ProjectExcluding(exclusion);

            if (sortOrder != Sort.None && sortBy != null)
            {
                cmd = cmd.Sort(
                    sortBy,
                    sortOrder == Sort.Ascending
                    ? Order.Ascending
                    : Order.Descending);
            }

            if (skip > 0)
                cmd.Skip(skip);

            if (take > 0)
                cmd.Limit(take);

            return await cmd.ExecuteAsync();
        }

        //SAVE

        /// <summary>
        /// Persists an entity to the database replacing the existing data.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns>The ID of the saved entity</returns>
        public static string Save(TEntity entity)
        {
            entity.Save();
            return entity.ID;
        }

        /// <summary>
        /// Persists an entity to the database replacing the existing data, while preserving some property values from the database. 
        /// The properties to be preserved can be specified with a 'New' expression or [Preserve] attributes.
        /// <para>TIP: Only root level properties are allowed in the 'New' expression.</para>
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <param name="preservation">x => new { x.PropOne, x.PropTwo }</param>
        public static string SavePreserving(TEntity entity, Expression<Func<TEntity, object>> preservation = null)
        {
            entity.SavePreserving(preservation);
            return entity.ID;
        }

        /// <summary>
        /// Persists an entity to the database replacing the existing data.
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <returns>The ID of the saved entity</returns>
        public static async Task<string> SaveAsync(TEntity entity)
        {
            await entity.SaveAsync();
            return entity.ID;
        }

        /// <summary>
        /// Persists an entity to the database replacing the existing data, while preserving some property values from the database. 
        /// The properties to be preserved can be specified with a 'New' expression or [Preserve] attributes.
        /// <para>TIP: Only root level properties are allowed in the 'New' expression.</para>
        /// </summary>
        /// <param name="entity">The entity to save</param>
        /// <param name="preservation">x => new { x.PropOne, x.PropTwo }</param>
        public static async Task<string> SavePreservingAsync(TEntity entity, Expression<Func<TEntity, object>> preservation = null)
        {
            await entity.SavePreservingAsync(preservation);
            return entity.ID;
        }

        //DELETE

        /// <summary>
        /// Delete an entity by ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        public static void Delete(string id)
        {
            DB.Delete<TEntity>(id);
        }

        /// <summary>
        /// Delete an entity by ID
        /// </summary>
        /// <param name="id">The ID of the entity to delete</param>
        public static async Task DeleteAsync(string id)
        {
            await DB.DeleteAsync<TEntity>(id);
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public static void Delete(TEntity entity)
        {
            entity.Delete();
        }

        /// <summary>
        /// Delete an entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        public static async Task DeleteAsync(TEntity entity)
        {
            await entity.DeleteAsync();
        }
    }
}
