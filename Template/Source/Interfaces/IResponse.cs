using MongoDB.Entities.Core;

namespace MongoWebApiStarter
{
    /// <summary>
    /// A response DTO to use when there's nothing to return from a service method
    /// </summary>
    public class Nothing : IResponse { }

    /// <summary>
    /// A response DTO interface that can self populate from a given domain entity
    /// </summary>
    /// <typeparam name="TEntity">The type of domain entity this response deals with</typeparam>
    public interface IResponse<TEntity> : IResponse where TEntity : IEntity
    {
        /// <summary>
        /// Populates the response from a given domain entity
        /// </summary>
        /// <param name="e">The domain entity to map the properties from</param>
        public void FromEntity(TEntity e);
    }

    /// <summary>
    /// Marker interface for response DTOs
    /// </summary>
    public interface IResponse { }
}
