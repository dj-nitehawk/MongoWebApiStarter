using MongoDB.Entities.Core;

namespace MongoWebApiStarter.Biz.Interfaces
{
    /// <summary>
    /// Interface for mapping data between two types
    /// </summary>
    /// <typeparam name="TModel">The type of model class</typeparam>
    /// <typeparam name="TEntity">The type of entity class</typeparam>
    public interface IMapper<TEntity> where TEntity : Entity
    {
        /// <summary>
        /// Maps a model to an entity
        /// </summary>
        /// <param name="model">The model instance</param>
        public TEntity ToEntity();

        /// <summary>
        /// Hydrates a model from a supplied entity
        /// </summary>
        /// <param name="entity">The entity instance</param>
        public void LoadFrom(TEntity entity);
    }
}
