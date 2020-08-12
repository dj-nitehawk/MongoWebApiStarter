using MongoDB.Entities;
using ServiceStack;

namespace MongoWebApiStarter
{
    /// <summary>
    /// Interface for Request DTOs that can create a domain entity from it's properties
    /// </summary>
    /// <typeparam name="TEntity">The type of domain entity this request deals with</typeparam>
    /// <typeparam name="TResponse">The type of the response DTO</typeparam>
    public interface IRequest<TEntity, TResponse> : IRequest<TResponse>, IReturn<TResponse>
        where TEntity : IEntity
        where TResponse : IResponse
    {
        /// <summary>
        /// Returns a domain entity by mapping properties of the request
        /// </summary>
        public TEntity ToEntity();
    }

    /// <summary>
    /// Marker interface for request DTOs
    /// </summary>    
    /// <typeparam name="TResponse">The type of the response DTO</typeparam>
    public interface IRequest<TResponse> : IReturn<TResponse> where TResponse : IResponse { }
}
